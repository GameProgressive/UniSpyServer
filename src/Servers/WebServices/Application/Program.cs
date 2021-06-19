using Serilog.Events;
using System;
using System.IO;
using System.Reflection;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Logging;
using WebServices.Application;

namespace WebServices
{
    public class Program
    {
        private static WebServerFactory Manager;
        public static void Main(string[] args)
        {
            //the working directory is different than binary execute file directory
            //we set the working directory as same as binary execute directory so we can read RetroSpyConfig.json
            var executeDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            Directory.SetCurrentDirectory(executeDirectory);

            try
            {
                Manager = new WebServerFactory();
                Manager.Start();
            }
            catch (UniSpyExceptionBase e)
            {
                LogWriter.ToLog(e);
            }
            catch (Exception e)
            {
                LogWriter.ToLog(LogEventLevel.Error, e.ToString());
            }
        }
    }
}
