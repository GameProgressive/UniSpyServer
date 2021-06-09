using Serilog;
using Serilog.Events;
using System;
using System.Linq;
using System.Net;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Encryption;
using UniSpyLib.Extensions;
using UniSpyLib.UniSpyConfig;

namespace UniSpyLib.Logging
{

    /// <summary>
    /// Provides an object wrapper for a file that is used to
    /// store LogMessage's into. Uses a Multi-Thread safe Queueing
    /// system, and provides full Asynchronous writing and flushing
    /// </summary>
    public class LogWriter
    {
        static LogWriter()
        {
            SettngUpLogger();
        }

        public static void SettngUpLogger()
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
                .WriteTo.File(
                path: $"Logs/[{UniSpyServerFactoryBase.ServerName}]-.log",
                outputTemplate: "{Timestamp:[yyyy-MM-dd HH:mm:ss]} [{Level:u4}] {Message:}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day)
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
                    Log.Verbose(message);
                    break;
                case LogEventLevel.Information:
                    Log.Information(message);
                    break;
                case LogEventLevel.Debug:
                    Log.Debug(message);
                    break;
                case LogEventLevel.Error:
                    Log.Error(message);
                    break;
                case LogEventLevel.Fatal:
                    Log.Fatal(message);
                    break;
                case LogEventLevel.Warning:
                    Log.Warning(message);
                    break;
            }
        }

        public static void ToLog(Exception e) => ToLog(LogEventLevel.Error, e.Message);
        public static void ToLog(string message) => ToLog(LogEventLevel.Information, message);
        public static void LogUnkownRequest(string data) => ToLog(LogEventLevel.Error, $"[Unknown] {data}");
        public static void LogUnkownRequest(byte[] data)
        => ToLog(LogEventLevel.Error, $"[Unknown] {StringExtensions.ReplaceUnreadableCharToHex(data)}");
        public static void LogCurrentClass(object param)
        => ToLog(LogEventLevel.Verbose, $"[ => ] [{param.GetType().Name}]");
        public static void LogNetworkMultiCast(string buffer)
        => ToLog(LogEventLevel.Debug, $"[Muti] {StringExtensions.ReplaceUnreadableCharToHex(buffer)}");
        public static void LogNetworkSending(IPEndPoint endPoint, byte[] buffer)
        => LogNetworkTraffic("Send", endPoint, buffer);
        public static void LogNetworkSending(IPEndPoint endPoint, string buffer)
        => LogNetworkSending(endPoint, UniSpyEncoding.GetBytes(buffer));
        public static void LogNetworkReceiving(IPEndPoint endPoint, byte[] buffer)
        => LogNetworkTraffic("Recv", endPoint, buffer);
        public static void LogNetworkReceiving(IPEndPoint endPoint, string buffer)
        => LogNetworkReceiving(endPoint, UniSpyEncoding.GetBytes(buffer));
        public static void LogNetworkTraffic(string type, IPEndPoint endPoint, byte[] buffer, long size)
        => LogNetworkTraffic(type, endPoint, buffer.Take((int)size).ToArray());
        private static void LogNetworkTraffic(string type, IPEndPoint endPoint, byte[] buffer)
        => LogNetworkTraffic(type, endPoint, StringExtensions.ReplaceUnreadableCharToHex(buffer));
        private static void LogNetworkTraffic(string type, IPEndPoint endPoint, string buffer)
        => ToLog(LogEventLevel.Debug, $"[{type}] [{endPoint}] {buffer}");
        public static void LogNetworkSpam(IPEndPoint endPoint)
        => ToLog(LogEventLevel.Error, $"[Spam] [{endPoint}] spam we ignored!");
    }
}
