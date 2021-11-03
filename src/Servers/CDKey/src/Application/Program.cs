using System;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.CDkey.Application
{
    public class Program
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
