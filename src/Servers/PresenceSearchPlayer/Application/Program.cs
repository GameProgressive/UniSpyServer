using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Entity.Structure;
using UniSpyLib.Logging;
using Serilog.Events;
using System;

namespace PresenceSearchPlayer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //create a instance of ServerManager class
                new PSPServerManager(UniSpyServerName.PSP).Start();
                Console.Title = "RetroSpy Server " + UniSpyServerManagerBase.RetroSpyVersion;
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
