using GameSpyLib.Common;
using GameSpyLib.Logging;
using System;

namespace ServerBrowser.Application
{
    internal class Program
    {
        private static ServerManager Manager;

        private static void Main(string[] args)
        {

            try
            {
                //create a instance of ServerManager class
                Manager = new ServerManager(RetroSpyServerName.ServerBrowser);
                Manager.Start();
                Console.Title = "RetroSpy Server " + ServerManagerBase.RetroSpyVersion;
            }
            catch (Exception e)
            {
                LogWriter.ToLog(Serilog.Events.LogEventLevel.Error, e.ToString());
            }

            Console.WriteLine("Press < Q > to exit. ");
            while (Console.ReadKey().Key != ConsoleKey.Q) { }
        }
    }
}
