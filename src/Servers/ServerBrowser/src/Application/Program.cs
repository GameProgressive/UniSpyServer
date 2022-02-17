using System;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Factory;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.ServerBrowser.Application
{
    public sealed class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new ServerFactory("ServerBrowser").Start();
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
