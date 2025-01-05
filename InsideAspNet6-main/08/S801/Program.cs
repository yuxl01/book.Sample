using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

//注册日志
internal partial class Program
{
    private static void Main(string[] args)
    {
        var di = new ServiceCollection();
        //注入日志相关服务
        var services = di.AddLogging(builder => builder
                .SetMinimumLevel(LogLevel.Trace)
                .AddConsole()
                .AddDebug()
                //.AddJsonConsole()
                .AddSystemdConsole());

        var serviceProvider = services.BuildServiceProvider();

        //从容器获取 ILoggerFactory
        ILoggerFactory loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

        //通过ILoggerFactory 创建ILogger对象 3种方式
        ILogger logger = loggerFactory.CreateLogger<Program>();
        ILogger logger1 = loggerFactory.CreateLogger("Program");

        //从容器获取
        ILogger logger2 = serviceProvider.GetRequiredService<ILogger<Program>>();

        var levels = (LogLevel[])Enum.GetValues(typeof(LogLevel));
        levels = levels.Where(it => it != LogLevel.None).ToArray();
        var eventId = 1;
        Array.ForEach(levels, level => logger.Log(level, eventId++, "This is a/an {0} log message.", level));
        Console.Read();
    }
}