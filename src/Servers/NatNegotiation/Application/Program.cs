using GameSpyLib.Common;
using GameSpyLib.Logging;
using Serilog.Events;
using System;
namespace NatNegotiation.Application
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
                Manager = new ServerManager(RetroSpyServerName.NatNegotiation);
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
