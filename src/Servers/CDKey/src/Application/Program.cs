using System;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.CDKey.Application
{
    public class Program
    {
        static void Main(string[] args)
        {
            var n = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
            var type = Type.GetType($"{n}.Handler.CmdSwitcher");
            type = Type.GetType("UniSpyServer.Servers.CDKey.Handler.CmdSwitcher");
            Type.GetType($"{n}.Entity.Structure.Client");
            // object instance = Activator.CreateInstance(type);
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
