using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

internal partial class Program
{
    private static void Main(string[] args)
    {
        var cBuilder = new ConfigurationBuilder();
        var configuration = cBuilder.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("logging.json").Build();

        var di = new ServiceCollection();
        //注入日志相关服务
        var services = di.AddLogging(builder => builder
              .AddConfiguration(configuration)
                .AddConsole()
                .AddDebug());

        var serviceProvider = services.BuildServiceProvider();

        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

       // Log(loggerFactory, "Foo");
        //Log(loggerFactory, "Bar");
        //Log(loggerFactory, "Baz");

        Console.Read();
    }

    private static void Log(ILoggerFactory loggerFactory, string category)
    {
        var logger = loggerFactory.CreateLogger(category);
        var levels = (LogLevel[])Enum.GetValues(typeof(LogLevel));
        levels = levels.Where(it => it != LogLevel.None).ToArray();
        var eventId = 1;
        Array.ForEach(levels, level => logger.Log(level, eventId++, "输出消息的日志等级为：{0} .", level));
    }
}