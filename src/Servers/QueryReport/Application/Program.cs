using GameSpyLib.Abstraction.BaseClass;
using GameSpyLib.Entity.Enumerate;
using GameSpyLib.Logging;
using Serilog.Events;
using System;

namespace QueryReport.Application
{
    internal class Program
    {
        private static ServerManager Manager;

        private static void Main(string[] args)
        {
            try
            {
                //create a instance of ServerManager class
                Manager = new ServerManager(RetroSpyServerName.QueryReport);
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
