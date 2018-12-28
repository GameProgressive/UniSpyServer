using System;
using System.IO;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace GameSpyLib
{
    public class Logger
    {
        public static Serilog.Core.Logger logger { get; protected set; }

        public static void Create(string rootPath)
        {
            if (!Directory.Exists(Path.Combine(rootPath, "Logs")))
                Directory.CreateDirectory(Path.Combine(rootPath, "Logs"));

            logger = new LoggerConfiguration()
                     .MinimumLevel.Debug()
                     .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                     .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information).WriteTo.RollingFile(@"Logs\Info-{Date}.log"))
                     .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug).WriteTo.RollingFile(@"Logs\Debug-{Date}.log"))
                     .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning).WriteTo.RollingFile(@"Logs\Warning-{Date}.log"))
                     .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error).WriteTo.RollingFile(@"Logs\Error-{Date}.log"))
                     .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal).WriteTo.RollingFile(@"Logs\Fatal-{Date}.log"))
                     .WriteTo.RollingFile(@"Logs\Verbose-{Date}.log")
                     .CreateLogger();
        }

        public static void Dispose()
        {
            if (logger != null)
                logger.Dispose();

            logger = null;
        }

        public static void Debug(string msg)
        {
            logger.Debug(msg);
        }

        public static void Debug(Exception ex, string msg = "")
        {
            logger.Debug(ex, msg);
        }

        public static void Warn(string msg)
        {
            logger.Warning(msg);
        }

        public static void Error(string msg)
        {
            logger.Error(msg);
        }

        public static void Error(Exception ex, string msg = "")
        {
            logger.Error(ex, msg);
        }

        public static void Fatal(string msg)
        {
            logger.Fatal(msg);
        }

        public static void Fatal(Exception ex, string msg = "")
        {
            logger.Fatal(ex, msg);
        }

        public static void Info(string msg)
        {
            logger.Information(msg);
        }
    }
}
