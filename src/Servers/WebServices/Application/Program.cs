using UniSpyLib.Entity.Structure;
using UniSpyLib.Logging;
using Serilog.Events;
using System;
using System.IO;
using System.Reflection;
using WebServices.Application;

namespace WebServices
{
    public class Program
    {
        private static ServerManager Manager;
        public static void Main(string[] args)
        {

            //the working directory is different than binary execute file directory
            //we set the working directory as same as binary execute directory so we can read RetroSpyConfig.json
            var executeDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            Directory.SetCurrentDirectory(executeDirectory);

            try
            {
                Manager = new ServerManager(UniSpyServerName.Web);
                Manager.Start();
            }
            catch (Exception e)
            {
                LogWriter.ToLog(LogEventLevel.Error, e.ToString());
            }
        }
    }
}
