using Serilog.Events;
using System;
using System.Threading.Tasks;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Logging;

namespace CDKey.Application
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
                new CDKeyServerFactory().Start();

                Console.Title = "UniSpyServer " + UniSpyServerFactoryBase.UniSpyVersion;
            }
            catch (UniSpyExceptionBase e)
            {
                LogWriter.ToLog(e);
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
