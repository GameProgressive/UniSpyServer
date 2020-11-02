using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Entity.Structure;
using UniSpyLib.Logging;
using Serilog.Events;
using System;

namespace GameStatus.Application
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //create a instance of ServerManager class
                new GSServerManager(UniSpyServerName.GS).Start();
                Console.Title = "RetroSpy Server " + ServerManagerBase.RetroSpyVersion;
            }
            catch (Exception e)
            {
                LogWriter.ToLog(LogEventLevel.Error,e.ToString());
            }

            Console.WriteLine("Press < Q > to exit. ");
            while (Console.ReadKey().Key != ConsoleKey.Q) { }
        }
    }
}
