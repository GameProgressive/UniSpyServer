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
            Console.WriteLine(Directory.GetCurrentDirectory());
            var location = System.Reflection.Assembly.GetEntryAssembly().Location;
            var directory = Path.GetDirectoryName(location);
            Console.WriteLine(directory);
            //Log.Logger = new LoggerConfiguration()
            //.Enrich.FromLogContext()
            //.WriteTo.Console()
            //.CreateLogger();

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
