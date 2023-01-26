using System;
using System.Net;
using Serilog;
using Serilog.Events;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Factory;
using UniSpyServer.UniSpyLib.Config;
using UniSpyServer.UniSpyLib.Extensions;

namespace UniSpyServer.UniSpyLib.Logging
{
    /// <summary>
    /// Provides an object wrapper for a file that is used to
    /// store LogMessage's into. Uses a Multi-Thread safe Queueing
    /// system, and provides full Asynchronous writing and flushing
    /// </summary>
    public class LogWriter
    {
        public static readonly LoggerConfiguration LogConfig;
        static LogWriter()
        {
            LogConfig = new LoggerConfiguration();

            switch (ConfigManager.Config.MinimumLogLevel)
            {
                case LogEventLevel.Verbose:
                    LogConfig.MinimumLevel.Verbose();
                    break;
                case LogEventLevel.Information:
                    LogConfig.MinimumLevel.Information();
                    break;
                case LogEventLevel.Debug:
                    LogConfig.MinimumLevel.Debug();
                    break;
                case LogEventLevel.Warning:
                    LogConfig.MinimumLevel.Warning();
                    break;
                case LogEventLevel.Error:
                    LogConfig.MinimumLevel.Error();
                    break;
                case LogEventLevel.Fatal:
                    LogConfig.MinimumLevel.Fatal();
                    break;
            }
            LogConfig = LogConfig
                .WriteTo.Console(outputTemplate: "{Timestamp:[HH:mm:ss]} [{Level:u4}] {Message:}{NewLine}{Exception}")
                .WriteTo.File(
                path: $"Logs/[{ServerLauncherBase.ServerName}]-.log",
                outputTemplate: "{Timestamp:[yyyy-MM-dd HH:mm:ss]} [{Level:u4}] {Message:}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day);
            Log.Logger = LogConfig.CreateLogger();
        }

        public static void Info(string message) => Log.Information(message);
        public static void Verbose(string message) => Log.Verbose(message);
        public static void Debug(string message) => Log.Debug(message);
        public static void Error(string message) => Log.Error(message);
        public static void Fatal(string message) => Log.Fatal(message);
        public static void Warning(string message) => Log.Warning(message);

        public static void ToLog(Exception e) => Error(e.Message);
        public static void ToLog(string message) => Info(message);
        public static void LogCurrentClass(object param) => Verbose($"[ => ] [{param.GetType().Name}]");
        public static void LogNetworkMultiCast(string buffer) => Debug($"[Cast] {StringExtensions.ConvertNonprintableCharToHex(buffer)}");

    }
}
