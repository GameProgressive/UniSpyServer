using Serilog.Events;
using System;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Entity.Structure;
using UniSpyLib.Logging;

namespace QueryReport.Application
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //create a instance of ServerManager class
                new QRServerFactory(UniSpyServerName.QR).Start();
                Console.Title = "RetroSpy Server " + UniSpyServerFactoryBase.RetroSpyVersion;
            }
            catch (Exception e)
            {
                LogWriter.ToLog(LogEventLevel.Error, e.ToString());
            }

            Console.WriteLine("Press < Q > to exit. ");
            while (Console.ReadKey().Key != ConsoleKey.Q) { }
        }
    }
}
