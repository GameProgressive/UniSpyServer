using GameSpyLib.Common;
using GameSpyLib.Logging;
using System;

namespace CDKey.Application
{
    /// <summary>
    /// This class represents a RetroSpy Server program
    /// </summary>
    internal class Program
    {
        private static ServerManager Manager;

        private static void Main(string[] args)
        {

            try
            {
                //create a instance of ServerManager class
                Manager = new ServerManager(RetroSpyServerName.CDKEY);
                Manager.Start();
                Console.Title = "RetroSpy Server " + Manager.RetroSpyVersion;
            }
            catch (Exception e)
            {
                LogWriter.ToLog(Serilog.Events.LogEventLevel.Error, e.ToString());
            }

            Console.WriteLine("Press < Q > to exit... ");
            while (Console.ReadKey().Key != ConsoleKey.Q) { }
        }
    }
}
