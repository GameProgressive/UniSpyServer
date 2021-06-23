using Serilog.Events;
using System;
using System.Threading.Tasks;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Logging;
namespace NatNegotiation.Application
{
    /// <summary>
    /// This class represents a RetroSpy Server program
    /// </summary>
    internal sealed class Program
    {
        static async Task Main(string[] args)
        {

            try
            {
                //create a instance of ServerManager class
                var factory = new NNServerFactory();
                await factory.Start();
                Console.Title = "RetroSpy Server " + UniSpyServerFactoryBase.UniSpyVersion;
            }
            catch (UniSpyExceptionBase e)
            {
                LogWriter.ToLog(e);
            }
            catch (Exception e)
            {
                LogWriter.ToLog(LogEventLevel.Error, e.ToString());
            }

            Console.WriteLine("Press < Q > to exit. ");
            while (Console.ReadKey().Key != ConsoleKey.Q) { }
        }
    }
}
