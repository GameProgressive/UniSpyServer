using System;
using System.IO;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace GameSpyLib
{
    /// <summary>
    /// This class rapresents a Console and File logger
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// The Serilog logger
        /// </summary>
        public static Serilog.Core.Logger logger { get; protected set; }

        /// <summary>
        /// If this is setted to true, all socket IO operations will be wrote into the debugger
        /// Pariculary usefull when debugging GameSpy protocol
        /// </summary>
        public static bool DebugSocket { get; set; }

        /// <summary>
        /// Creates the Logger
        /// </summary>
        /// <param name="rootPath">Base folder to save the logs</param>
        public static void Create(string logPath)
        {
            logger = new LoggerConfiguration()
                     .MinimumLevel.Debug()
                     .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                     .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information).WriteTo.RollingFile(logPath + @"\Info-{Date}.log"))
                     .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug).WriteTo.RollingFile(logPath + @"\Debug-{Date}.log"))
                     .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning).WriteTo.RollingFile(logPath + @"\Warning-{Date}.log"))
                     .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error).WriteTo.RollingFile(logPath + @"\Error-{Date}.log"))
                     .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal).WriteTo.RollingFile(logPath + @"\Fatal-{Date}.log"))
                     .WriteTo.RollingFile(logPath + @"\Verbose-{Date}.log")
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
