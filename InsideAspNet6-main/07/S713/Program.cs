﻿using App;
using System;
using System.Diagnostics;
using System.Net;

DiagnosticListener.AllListeners.Subscribe(OnSubscribe);


var source = new DiagnosticListener("Web");
var stopwatch = Stopwatch.StartNew();
if (source.IsEnabled("ReceiveRequest"))
{
    var request = new HttpRequestMessage(HttpMethod.Get, "https://www.artech.top");
    source.Write("ReceiveRequest", new
    {
        Request = request,
        Timestamp = Stopwatch.GetTimestamp()
    });
}
await Task.Delay(100);
if (source.IsEnabled("SendReply"))
{
    var response = new HttpResponseMessage(HttpStatusCode.OK);
    source.Write("SendReply", new
    {
        Response = response,
        Elaped = stopwatch.Elapsed
    });
}

void OnSubscribe(DiagnosticListener listener)
{
    if (listener.Name.Equals("Web"))
    {
        listener.SubscribeWithAdapter(new DiagnosticCollector());
    }
}
