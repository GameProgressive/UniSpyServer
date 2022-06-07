using System;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Factory;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.WebServer.Application
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
                LogWriter.ToLog(e);
            }

            Console.WriteLine("Press < Q > to exit. ");
            while (Console.ReadKey().Key != ConsoleKey.Q) { }
        }
    }
}