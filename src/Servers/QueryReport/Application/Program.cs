using Serilog.Events;
using System;
using System.Threading.Tasks;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass.Factory;
using UniSpyLib.Logging;

namespace QueryReport.Application
{
    internal sealed class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //create a instance of ServerManager class
                new QRServerFactory().Start();

            }

            catch (Exception e)
            {
                LogWriter.ToLog(e);
            }

            Console.WriteLine("Press < Q > to exit. ");
            while (Console.ReadKey().Key != ConsoleKey.Q) { }
        }
    }
}
