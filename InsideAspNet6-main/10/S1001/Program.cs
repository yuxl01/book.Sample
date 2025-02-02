﻿using App;
using Microsoft.Extensions.ObjectPool;

var objectPool = ObjectPool.Create<FoobarService>();
while (true)
{
    Console.Write("Used services: ");
    await Task.WhenAll(Enumerable.Range(1, 3).Select(_ => ExecuteAsync()));
    Console.Write("\n");
}
async Task ExecuteAsync()
{
    var service = objectPool.Get();
    try
    {
        Console.Write($"{service.Id}; ");
        Console.Write($"{service.GetHashCode()}; ");
        await Task.Delay(1000);
    }
    finally
    {
        objectPool.Return(service);
    }
}
