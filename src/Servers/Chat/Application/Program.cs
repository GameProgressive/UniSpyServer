using System;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass.Factory;
using UniSpyLib.Logging;

namespace Chat.Application
{
    /// <summary>
    /// This class represents a UniSpyServer program
    /// </summary>
    internal static class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //create a instance of ServerManager class
                new ChatServerFactory().Start();

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
