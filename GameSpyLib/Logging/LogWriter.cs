using GameSpyLib.Common;
using GameSpyLib.RetroSpyConfig;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;

namespace GameSpyLib.Logging
{

    /// <summary>
    /// Provides an object wrapper for a file that is used to
    /// store LogMessage's into. Uses a Multi-Thread safe Queueing
    /// system, and provides full Asynchronous writing and flushing
    /// </summary>
    public class LogWriter
    {

        public Logger Log { get; protected set; }

        public LogWriter(string serverName)
        {
            switch (ConfigManager.Config.MinimumLogLevel)
            {
                case LogEventLevel.Verbose:
                    Log = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console(
               outputTemplate: "{Timestamp:[HH:mm:ss]} [{Level:u4}] {Message:}{NewLine}{Exception}")
                .WriteTo.File($"Logs/[{serverName}]-.log",
                outputTemplate: "{Timestamp:[yyyy-MM-dd HH:mm:ss]} [{Level:u4}] {Message:}{NewLine}{Exception}", rollingInterval: RollingInterval.Day)
                .CreateLogger();
                    break;
                case LogEventLevel.Information:
                    Log = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console(
               outputTemplate: "{Timestamp:[HH:mm:ss]} [{Level:u4}] {Message:}{NewLine}{Exception}")
                .WriteTo.File($"Logs/[{serverName}]-.log",
                outputTemplate: "{Timestamp:[yyyy-MM-dd HH:mm:ss]} [{Level:u4}] {Message:}{NewLine}{Exception}", rollingInterval: RollingInterval.Day)
                .CreateLogger();
                    break;
                case LogEventLevel.Debug:
                    Log = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(
               outputTemplate: "{Timestamp:[HH:mm:ss]} [{Level:u4}] {Message:}{NewLine}{Exception}")
                .WriteTo.File($"Logs/[{serverName}]-.log",
                outputTemplate: "{Timestamp:[yyyy-MM-dd HH:mm:ss]} [{Level:u4}] {Message:}{NewLine}{Exception}", rollingInterval: RollingInterval.Day)
                .CreateLogger();
                    break;
                case LogEventLevel.Warning:
                    Log = new LoggerConfiguration()
                .MinimumLevel.Warning()
                .WriteTo.Console(
               outputTemplate: "{Timestamp:[HH:mm:ss]} [{Level:u4}] {Message:}{NewLine}{Exception}")
                .WriteTo.File($"Logs/[{serverName}]-.log",
                outputTemplate: "{Timestamp:[yyyy-MM-dd HH:mm:ss]} [{Level:u4}] {Message:}{NewLine}{Exception}", rollingInterval: RollingInterval.Day)
                .CreateLogger();
                    break;
                case LogEventLevel.Error:
                    Log = new LoggerConfiguration()
                .MinimumLevel.Error()
                .WriteTo.Console(
               outputTemplate: "{Timestamp:[HH:mm:ss]} [{Level:u4}] {Message:}{NewLine}{Exception}")
                .WriteTo.File($"Logs/[{serverName}]-.log",
                outputTemplate: "{Timestamp:[yyyy-MM-dd HH:mm:ss]} [{Level:u4}] {Message:}{NewLine}{Exception}", rollingInterval: RollingInterval.Day)
                .CreateLogger();
                    break;
                case LogEventLevel.Fatal:
                    Log = new LoggerConfiguration()
                .MinimumLevel.Fatal()
                .WriteTo.Console(
               outputTemplate: "{Timestamp:[HH:mm:ss]} [{Level:u4}] {Message:}{NewLine}{Exception}")
                .WriteTo.File($"Logs/[{serverName}]-.log",
                outputTemplate: "{Timestamp:[yyyy-MM-dd HH:mm:ss]} [{Level:u4}] {Message:}{NewLine}{Exception}", rollingInterval: RollingInterval.Day)
                .CreateLogger();
                    break;
            }
        }

        /// <summary>
        /// Convient to print log
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        public static void ToLog(LogEventLevel level, string message)
        {
            switch (level)
            {
                case LogEventLevel.Verbose:
                    ServerManagerBase.LogWriter.Log.Verbose(message);
                    break;
                case LogEventLevel.Information:
                    ServerManagerBase.LogWriter.Log.Information(message);
                    break;
                case LogEventLevel.Debug:
                    ServerManagerBase.LogWriter.Log.Debug(message);
                    break;
                case LogEventLevel.Error:
                    ServerManagerBase.LogWriter.Log.Error(message);
                    break;
                case LogEventLevel.Fatal:
                    ServerManagerBase.LogWriter.Log.Fatal(message);
                    break;
                case LogEventLevel.Warning:
                    ServerManagerBase.LogWriter.Log.Warning(message);
                    break;
            }
        }

        public static void ToLog(string message)
        {
            ToLog(LogEventLevel.Information, message);
        }
        public static void WriteException(Exception e)
        {
            ToLog(e.ToString());
        }
    }

}
