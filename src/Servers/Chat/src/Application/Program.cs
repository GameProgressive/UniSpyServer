using System;
using UniSpy.Server.Core.Logging;

namespace UniSpy.Server.Chat.Application
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new ServerLauncher().Start();
                Console.WriteLine("Press < Q > to exit. ");
                while (Console.ReadKey().Key != ConsoleKey.Q) { }
            }
            catch (System.Exception e)
            {
                LogWriter.LogError(e);
            }
        }
    }
}
