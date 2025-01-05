using App;
using System.Data;
using System.Diagnostics;

var observer = new Observer<DiagnosticListener>(OnNext);

DiagnosticListener.AllListeners.Subscribe(observer);

var source = new DiagnosticListener("MyDiagnosticTest");
if (source.IsEnabled("CommandExecution"))
{
    source.Write("CommandExecution",
        new
        {
            CommandType = CommandType.Text,
            CommandText = "SELECT * FROM T_USER"
        });
}

static void OnNext(DiagnosticListener listener)
{
    if (listener.Name.Equals("MyDiagnosticTest"))
    {
        listener.Subscribe(new Observer<KeyValuePair<string, object?>>(eventData =>
        {
            Console.WriteLine($"Event Name: {eventData.Key}");
            if (eventData.Value != null)
            {
                dynamic payload = eventData.Value;
                Console.WriteLine($"CommandType: {payload.CommandType}");
                Console.WriteLine($"CommandText: {payload.CommandText}");
            }
        }));
    }
}