using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

var di = new ServiceCollection();
//注入日志相关服务
var services = di.AddLogging(builder => builder
        .AddConsole()
       .AddSimpleConsole(options => options.IncludeScopes = true));
var serviceProvider = services.BuildServiceProvider();


var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
using (logger.BeginScope($"Foobar Transaction[{Guid.NewGuid()}]"))
{
    var stopwatch = Stopwatch.StartNew();
    await Task.Delay(500);
    logger.LogInformation("操作 foo 完成耗时  {0}", stopwatch.Elapsed);

    await Task.Delay(300);
    logger.LogInformation("操作 bar 完成耗时 {0}", stopwatch.Elapsed);

    await Task.Delay(800);
    logger.LogInformation("操作 baz 完成耗时 {0}", stopwatch.Elapsed);
}

Console.Read();