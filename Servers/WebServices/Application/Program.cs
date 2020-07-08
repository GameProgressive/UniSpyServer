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
            //Log.Logger = new LoggerConfiguration()
            //.Enrich.FromLogContext()
            //.WriteTo.Console()
            //.CreateLogger();

            //try
            //{
            //   // Log.Information("Starting up");

            //    var host = new WebHostBuilder()
            //            .UseKestrel(x => x.AllowSynchronousIO = true)
            //            .UseUrls("http://*:80")
            //            .UseContentRoot(Directory.GetCurrentDirectory())
            //            .UseStartup<Startup>()
            //            .ConfigureLogging(logBuilder =>
            //            {
            //                logBuilder.ClearProviders();
            //                logBuilder.AddDebug();
            //                logBuilder.AddConsole();
            //            })
            //            .Build();

            //    host.Run();
            //}
            //catch (Exception ex)
            //{
            //  //  Log.Fatal(ex, "Application start-up failed");
            //}
            //finally
            //{
            //   // Log.CloseAndFlush();
            //}
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
