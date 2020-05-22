using System;
using GameSpyLib.Common;
using GameSpyLib.Logging;
using Serilog.Events;
using WebServices.Application;

namespace WebServices
{
   
    class Program
    {
        private static ServerManager Manager;
        static void Main(string[] args)
        {
            try
            {
                //create a instance of ServerManager class
                Manager = new ServerManager(RetroSpyServerName.WebServer);
                Manager.Start();
                Console.Title = "RetroSpy Server " + ServerManagerBase.RetroSpyVersion;
            }
            catch (Exception e)
            {
                LogWriter.ToLog(LogEventLevel.Error, e.ToString());
            }

            Console.WriteLine("Press < Q > to exit... ");
            while (Console.ReadKey().Key != ConsoleKey.Q) { }
        }
    }
}
