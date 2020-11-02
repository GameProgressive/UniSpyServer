using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Extensions;
using UniSpyLib.RetroSpyConfig;
using Serilog;
using Serilog.Events;
using System;

namespace UniSpyLib.Logging
{

    /// <summary>
    /// Provides an object wrapper for a file that is used to
    /// store LogMessage's into. Uses a Multi-Thread safe Queueing
    /// system, and provides full Asynchronous writing and flushing
    /// </summary>
    public class LogWriter
    {

        //private static Logger Log;
        static LogWriter()
        {
            LoggerConfiguration logConfig = new LoggerConfiguration();

            switch (ConfigManager.Config.MinimumLogLevel)
            {
                case LogEventLevel.Verbose:
                    logConfig.MinimumLevel.Verbose();
                    break;
                case LogEventLevel.Information:
                    logConfig.MinimumLevel.Information();
                    break;
                case LogEventLevel.Debug:
                    logConfig.MinimumLevel.Debug();
                    break;
                case LogEventLevel.Warning:
                    logConfig.MinimumLevel.Warning();
                    break;
                case LogEventLevel.Error:
                    logConfig.MinimumLevel.Error();
                    break;
                case LogEventLevel.Fatal:
                    logConfig.MinimumLevel.Fatal();
                    break;
            }
            Log.Logger = logConfig
                .WriteTo.Console(outputTemplate: "{Timestamp:[HH:mm:ss]} [{Level:u4}] {Message:}{NewLine}{Exception}")
                .WriteTo.File($"Logs/[{ServerManagerBase.ServerName}]-.log",
                outputTemplate: "{Timestamp:[yyyy-MM-dd HH:mm:ss]} [{Level:u4}] {Message:}{NewLine}{Exception}", rollingInterval: RollingInterval.Day)
                .CreateLogger();
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
                    Log.Verbose($"[{ServerManagerBase.ServerName}] " + message);
                    break;
                case LogEventLevel.Information:
                    Log.Information($"[{ServerManagerBase.ServerName}] " + message);
                    break;
                case LogEventLevel.Debug:
                    Log.Debug($"[{ServerManagerBase.ServerName}] " + message);
                    break;
                case LogEventLevel.Error:
                    Log.Error($"[{ServerManagerBase.ServerName}] " + message);
                    break;
                case LogEventLevel.Fatal:
                    Log.Fatal($"[{ServerManagerBase.ServerName}] " + message);
                    break;
                case LogEventLevel.Warning:
                    Log.Warning($"[{ServerManagerBase.ServerName}] " + message);
                    break;
            }
        }

        public static void ToLog(Exception e)
        {
            ToLog(LogEventLevel.Error, e.ToString());
        }

        public static void ToLog(string message)
        {
            ToLog(LogEventLevel.Information, message);
        }

        public static void UnknownDataRecieved(string data)
        {
            ToLog(LogEventLevel.Error, $"[Unknown] {data}");
        }
        public static void UnknownDataRecieved(byte[] data)
        {
            ToLog(LogEventLevel.Error, $"[Unknown] {StringExtensions.ReplaceUnreadableCharToHex(data)}");
        }

        public static void LogCurrentClass(object param)
        {
            ToLog(LogEventLevel.Verbose, $"[ => ] [{param.GetType().Name}]");
        }
    }

}
