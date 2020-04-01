using GameSpyLib.Logging;
using System;

namespace ServerBrowser.Application
{
    internal class Program
    {
        public static readonly string ServerName = "SB";

        private static ServerManager Manager;

        private static void Main(string[] args)
        {

            try
            {
                //create a instance of ServerManager class
                Manager = new ServerManager(ServerName);
                Console.Title = "RetroSpy Server " + Manager.RetroSpyVersion;
            }
            catch (Exception e)
            {
                LogWriter.ToLog(Serilog.Events.LogEventLevel.Error, e.ToString());
            }
            Console.ReadKey();

        }
    }
}
