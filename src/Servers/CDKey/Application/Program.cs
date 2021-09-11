using System;
using CDKey.Application;
using UniSpyLib.Logging;

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