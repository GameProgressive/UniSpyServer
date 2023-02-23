using System;
using Serilog;
using Serilog.Events;
using UniSpy.Server.Core.Abstraction.BaseClass.Factory;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Config;
using UniSpy.Server.Core.Extension;

namespace UniSpy.Server.Core.Logging
{
    /// <summary>
    /// Provides an object wrapper for a file that is used to
    /// store LogMessage's into. Uses a Multi-Thread safe Queueing
    /// system, and provides full Asynchronous writing and flushing
    /// </summary>
    public static class LogWriter
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
        public static void LogVerbose(string message) => Log.Verbose(message);
        public static void LogDebug(string message) => Log.Debug(message);
        public static void LogInfo(string message) => Log.Information(message);
        public static void LogWarn(string message) => Log.Warning(message);
        public static void LogError(string message) => Log.Error(message);
        public static void LogError(Exception e) => LogError(e.Message);
        public static void LogFatal(string message) => Log.Fatal(message);
        public static void LogCurrentClass(object param) => LogVerbose($"[ => ] [{param.GetType().Name}]");
        public static void LogCurrentClass(this IClient client, object param) => LogVerbose(FormatLogMessage(client, $"[ => ] [{param.GetType().Name}]"));

        public static string FormatLogMessage(this IClient client, string message) => $"[{client.Connection.RemoteIPEndPoint}] {message}";
        public static string FormatNetworkLogMessage(string type, string message) => $"[{type}] {message}";
        public static string FormatNetworkLogMessage(string type, byte[] message, bool isLogRaw)
        {
            if (isLogRaw)
            {
                // we first show printable bytes, then show all bytes
                var tempLog = $"{StringExtensions.ConvertPrintableBytesToString(message)} [{StringExtensions.ConvertByteToHexString(message)}]";
                return $"[{type}] {tempLog}";
            }
            else
            {
                var tempLog = StringExtensions.ConvertNonprintableBytesToHexString(message);
                return $"[{type}] {tempLog}";
            }
        }

        public static void LogVerbose(this IClient client, string message) => Log.Verbose(FormatLogMessage(client, message));
        public static void LogInfo(this IClient client, string message) => Log.Information(FormatLogMessage(client, message));
        public static void LogDebug(this IClient client, string message) => Log.Debug(FormatLogMessage(client, message));
        public static void LogWarn(this IClient client, string message) => Log.Warning(FormatLogMessage(client, message));
        public static void LogError(this IClient client, string message) => Log.Error(FormatLogMessage(client, message));
        public static void LogError(this IClient client, Exception e) => Log.Error(FormatLogMessage(client, e.Message));
        public static void LogFatal(this IClient client, string message) => Log.Fatal(FormatLogMessage(client, message));

        public static void LogNetworkReceiving(this IClient client, string message) => LogDebug(client, FormatNetworkLogMessage("Recv", message));
        public static void LogNetworkSending(this IClient client, string message) => LogDebug(client, FormatNetworkLogMessage("Send", message));
        public static void LogNetworkReceiving(this IClient client, byte[] message) => LogDebug(client, FormatNetworkLogMessage("Recv", message, client.IsLogRaw));
        public static void LogNetworkSending(this IClient client, byte[] message) => LogDebug(client, FormatNetworkLogMessage("Send", message, client.IsLogRaw));
        public static void LogNetworkMultiCast(this IClient client, string message) => LogDebug(client, FormatNetworkLogMessage("Cast", message));
        public static void LogNetworkMultiCast(this IClient client, byte[] message) => LogDebug(client, FormatNetworkLogMessage("Cast", message, client.IsLogRaw));
    }
}
