using Serilog.Events;
using System;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Entity.Structure;
using UniSpyLib.Logging;

namespace ServerBrowser.Application
{
    internal class Program
    {
        static void Main(string[] args)
        {

            try
            {
                new SBServerFactory().Start();
                Console.Title = "RetroSpy Server " + UniSpyServerFactoryBase.UniSpyVersion;
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
