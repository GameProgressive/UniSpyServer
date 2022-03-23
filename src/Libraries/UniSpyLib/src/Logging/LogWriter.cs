using System;
using System.Linq;
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
        static LogWriter()
        {
            SettngUpLogger();
        }

        public static void SettngUpLogger()
        {
            var logConfig = new LoggerConfiguration();

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
                path: $"Logs/[{ServerFactory.ServerName}]-.log",
                outputTemplate: "{Timestamp:[yyyy-MM-dd HH:mm:ss]} [{Level:u4}] {Message:}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        public static void Info(string message) => Log.Information(message);
        public static void Verbose(string message) => Log.Verbose(message);
        public static void Debug(string message) => Log.Debug(message);
        public static void Error(string message) => Log.Error(message);
        public static void Fatal(string message) => Log.Fatal(message);
        public static void Warning(string message) => Log.Warning(message);

        public static void ToLog(Exception e) => Error(e.Message);
        public static void ToLog(string message) => Info(message);
        public static void LogUnkownRequest(string data) => Error($"[Unknown] {data}");
        public static void LogUnkownRequest(byte[] data) => Error($"[Unknown] {StringExtensions.ReplaceUnreadableCharToHex(data)}");
        public static void LogCurrentClass(object param) => Verbose($"[ => ] [{param.GetType().Name}]");
        public static void LogNetworkMultiCast(string buffer) => Debug($"[Muti] {StringExtensions.ReplaceUnreadableCharToHex(buffer)}");
        public static void LogNetworkSending(IPEndPoint endPoint, byte[] buffer) => LogNetworkTraffic("Send", endPoint, buffer);
        public static void LogNetworkSending(IPEndPoint endPoint, string buffer) => LogNetworkTraffic("Send", endPoint, buffer);
        public static void LogNetworkReceiving(IPEndPoint endPoint, byte[] buffer) => LogNetworkTraffic("Recv", endPoint, buffer);
        public static void LogNetworkReceiving(IPEndPoint endPoint, string buffer) => LogNetworkTraffic("Recv", endPoint, buffer);
        private static void LogNetworkTraffic(string type, IPEndPoint endPoint, byte[] buffer) => LogNetworkTraffic(type, endPoint, StringExtensions.FormatBytes(buffer));
        private static void LogNetworkTraffic(string type, IPEndPoint endPoint, string buffer) => Debug($"[{type}] [{endPoint}] {buffer}");
        public static void LogNetworkSpam(IPEndPoint endPoint) => Error($"[Spam] [{endPoint}] spam we ignored!");
        public static void LogNetworkTransit(IPEndPoint sender, IPEndPoint receiver, byte[] buffer) => Verbose($"[{sender}]=>[{receiver}] {buffer}");
    }
}
