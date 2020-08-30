using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System;
using Serilog;
using WebServices.Application;
using GameSpyLib.Common;
using GameSpyLib.Logging;
using Serilog.Events;

namespace WebServices
{
    public class Program
    {
        private static ServerManager Manager;
        public static void Main(string[] args)
        {
            //the working directory is different than binary execute file directory
            //you have to put RetroSpyConfig.json to project folder
            //currently i can not fix this problem
            Console.WriteLine(Directory.GetCurrentDirectory());
            var location = System.Reflection.Assembly.GetEntryAssembly().Location;
            var directory = Path.GetDirectoryName(location);

            Console.WriteLine(directory);
            try
            {
                Manager = new ServerManager(RetroSpyServerName.WebServices);
                Manager.Start();
            }
            catch (Exception e)
            {
                LogWriter.ToLog(LogEventLevel.Error, e.ToString());
            }
        }
    }
}
