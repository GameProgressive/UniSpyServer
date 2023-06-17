using System;
using UniSpy.Server.Core.Logging;

namespace UniSpy.Server.ServerBrowser.Application
{
    public sealed class Program
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
                UniSpy.Exception.HandleException(e);
            }
        }
    }
}
