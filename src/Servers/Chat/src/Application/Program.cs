using System;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Factory;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.Chat.Application
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new ServerFactory("Chat").Start();
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
