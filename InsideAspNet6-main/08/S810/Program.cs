using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

var random = new Random();
var template = @"方法 FoobarAsync 被调用完成.
    参数: foo={foo}, bar={bar}
    返回值: {returnValue}
    时间:{time}";

var log = LoggerMessage.Define<int, long, double, TimeSpan>(
    logLevel: LogLevel.Information,
    eventId: 3721,
    formatString: template);

var di = new ServiceCollection();
//注入日志相关服务
var services = di.AddLogging(builder => builder
        .AddConsole());

var serviceProvider = services.BuildServiceProvider();
var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

await FoobarAsync(random.Next(), random.Next());
await FoobarAsync(random.Next(), random.Next());
Console.Read();

async Task<double> FoobarAsync(int foo, long bar)
{
    var stopwatch = Stopwatch.StartNew();
    await Task.Delay(random.Next(100, 900));
    var result = random.Next();
    log(logger, foo, bar, result, stopwatch.Elapsed, null);
    return result;
}