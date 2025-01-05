using System.Diagnostics.Tracing;

namespace App
{
public class DatabaseSourceListener : EventListener
{
    protected override void OnEventSourceCreated(EventSource eventSource)
    {
        if (eventSource.Name == "事件发射源")
        {
            EnableEvents(eventSource, EventLevel.LogAlways);
        }
    }

    protected override void OnEventWritten(EventWrittenEventArgs eventData)
    {
        Console.WriteLine($"EventId: {eventData.EventId}");
        Console.WriteLine($"EventName: {eventData.EventName}");
        Console.WriteLine($"Payload");
        var index = 0;

        if (eventData.PayloadNames != null)
        {
            foreach (var payloadName in eventData.PayloadNames)
            {
                Console.WriteLine($"\t{payloadName}:{eventData.Payload?[index++]}");
            }
        }
    }
}
}