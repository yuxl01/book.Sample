//using App;
//using System.Data;
//using System.Diagnostics.Tracing;

//_= new DatabaseSourceListener();
//DatabaseSource.Instance.OnCommandExecute(CommandType.Text, "SELECT * FROM T_USER");

using App;
using System.Data;
using System.Diagnostics.Tracing;
class Program
{
    static void Main(string[] args)
    {
        DatabaseSource.Instance.OnCommandExecute(CommandType.Text, "SELECT * FROM T_USER");
        //using (var listener = new DatabaseSourceListener())
        //{
           
        //}
    }
}