using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Debug;

internal partial class Program
{
    private static void Main(string[] args)
    {
        var di = new ServiceCollection();
        //注入日志相关服务
        var services = di.AddLogging(builder => builder
              .AddConsole()
              .AddDebug()
              .AddFilter(FilterSpecifyProvider));

        var serviceProvider = services.BuildServiceProvider();

        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

        Log(loggerFactory, "Foo");
        Log(loggerFactory, "Bar");
        Log(loggerFactory, "Baz");

        Console.Read();
    }

    //输出日志
    private static void Log(ILoggerFactory loggerFactory, string category)
    {
        var logger = loggerFactory.CreateLogger(category);
        var levels = (LogLevel[])Enum.GetValues(typeof(LogLevel));
        levels = levels.Where(it => it != LogLevel.None).ToArray();
        var eventId = 1;
        Array.ForEach(levels, level => logger.Log(level, eventId++, "输出消息的日志等级为：{0} .", level));
    }

    //对所有的ILoggerProvider，过滤的回调 Func<string, LogLevel, bool> categoryLevelFilter
    private static bool Filter(string category, LogLevel level)
    {
        return category switch
        {
            "Foo" => level >= LogLevel.Debug,
            "Bar" => level >= LogLevel.Warning,
            "Baz" => level >= LogLevel.Error,
            _ => level >= LogLevel.Information,
        };
    }

    //对具体的ILoggerProvider，过滤的回调 Func<string, LogLevel, bool> categoryLevelFilter
    private static bool FilterSpecifyProvider(string provider, string category, LogLevel level)=> provider switch
    {
        //对ConsoleLoggerProvider 输出的日志进行针对性的输出
        var p when p == typeof(ConsoleLoggerProvider).FullName => level >= LogLevel.Debug,
        var p when p == typeof(DebugLoggerProvider).FullName => level >= LogLevel.Error,
        _ => true,
    };
}