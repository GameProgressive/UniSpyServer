using System;
using UniSpyLib.Logging;

namespace PresenceSearchPlayer.Application
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new ServerFactory().Start();
            }
            catch (Exception e)
            {
                LogWriter.ToLog(e);
            }

            Console.WriteLine("Press < Q > to exit. ");
            while (Console.ReadKey().Key != ConsoleKey.Q) { }
        }
    }
}

