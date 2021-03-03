using Serilog.Events;
using System;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Entity.Structure;
using UniSpyLib.Logging;

namespace PresenceConnectionManager.Application
{
    /// <summary>
    /// This class represents a RetroSpy Server program
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                //create a instance of ServerManager class
                new PCMServerFactory().Start();
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
