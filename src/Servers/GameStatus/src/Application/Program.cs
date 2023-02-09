using System;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.GameStatus.Application
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new ServerLauncher().Start();
            }
            catch (Exception e)
            {
                LogWriter.LogError(e);
            }

            Console.WriteLine("Press < Q > to exit. ");
            while (Console.ReadKey().Key != ConsoleKey.Q) { }
        }
    }
}
