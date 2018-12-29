using System;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace GameSpyLib.Logging
{
    /// <summary>
    /// This class writes the Log using Serilog
    /// </summary>
    public class LogWriter
    {
        /// <summary>
        /// If this is setted to true, all socket IO operations will be wrote into the debugger
        /// Pariculary usefull when debugging GameSpy protocol
        /// </summary>
        public static bool DebugSocket { get; set; }

        /// <summary>
        /// Creates the Logger
        /// </summary>
        /// <param name="rootPath">Base folder to save the logs</param>
        public static void Create(string logPath, bool debug)
        {
            LoggerConfiguration cfg = new LoggerConfiguration();

            if (debug)
                cfg.MinimumLevel.Debug();
            else
                cfg.MinimumLevel.Information();

            cfg.WriteTo.Console(theme: AnsiConsoleTheme.Code);

            // Note: For some reason Async logger thows some exceptions in Log.CloseAndFlush(), We will be use buffered RollingFile for now
            /*cfg.WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information).WriteTo.Async(a => a.RollingFile(logPath + @"\RetroSpy-Info-{Date}.log", buffered: false)));
            cfg.WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug).WriteTo.Async(a => a.RollingFile(logPath + @"\RetroSpy-Debug-{Date}.log", buffered: false)));
            cfg.WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning).WriteTo.Async(a => a.RollingFile(logPath + @"\RetroSpy-Warning-{Date}.log", buffered: false)));
            cfg.WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error).WriteTo.Async(a => a.RollingFile(logPath + @"\RetroSpy-Error-{Date}.log", buffered: false)));
            cfg.WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal).WriteTo.Async(a => a.RollingFile(logPath + @"\RetroSpy-Fatal-{Date}.log", buffered: false)));*/

            cfg.WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information).WriteTo.RollingFile(logPath + @"\RetroSpy-Info-{Date}.log", buffered: true));
            cfg.WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug).WriteTo.RollingFile(logPath + @"\RetroSpy-Debug-{Date}.log", buffered: true));
            cfg.WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning).WriteTo.RollingFile(logPath + @"\RetroSpy-Warning-{Date}.log", buffered: true));
            cfg.WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error).WriteTo.RollingFile(logPath + @"\RetroSpy-Error-{Date}.log", buffered: true));
            cfg.WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal).WriteTo.RollingFile(logPath + @"\RetroSpy-Fatal-{Date}.log", buffered: true));

            Log.Logger = cfg.CreateLogger();
        }

        public static void Dispose()
        {
            Log.CloseAndFlush();
        }

        public static void Debug(string msg)
        {
            Log.Debug(msg);
        }

        public static void Debug(Exception ex, string msg = "")
        {
            Log.Debug(ex, msg);
        }

        public static void Warn(string msg)
        {
            Log.Warning(msg);
        }

        public static void Error(string msg)
        {
            Log.Error(msg);
        }

        public static void Error(Exception ex, string msg = "")
        {
            Log.Error(ex, msg);
        }

        public static void Fatal(string msg)
        {
            Log.Fatal(msg);
        }

        public static void Fatal(Exception ex, string msg = "")
        {
            Log.Fatal(ex, msg);
        }

        public static void Info(string msg)
        {
            Log.Information(msg);
        }
    }
}
