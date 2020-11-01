using GameSpyLib.Abstraction.BaseClass;
using GameSpyLib.Entity.Structure;
using GameSpyLib.Logging;
using Serilog.Events;
using System;

namespace StatsTracking.Application
{
    internal class Program
    {
        private static STServerManager Manager;

        private static void Main(string[] args)
        {
            try
            {
                //create a instance of ServerManager class
                Manager = new STServerManager(RetroSpyServerName.GameStatus);
                Manager.Start();
                Console.Title = "RetroSpy Server " + ServerManagerBase.RetroSpyVersion;
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
