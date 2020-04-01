using GameSpyLib.Common;
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
            Log = new LoggerConfiguration()
                 .MinimumLevel.Verbose()
                 .WriteTo.Console(
                outputTemplate: "{Timestamp:[HH:mm:ss]} [{Level:u4}] {Message:}{NewLine}{Exception}")
                 .WriteTo.File($"Logs/[{serverName}]-.log",
                 outputTemplate: "{Timestamp:[yyyy-MM-dd HH:mm:ss]} [{Level:u4}] {Message:}{NewLine}{Exception}", rollingInterval: RollingInterval.Day)
                 .CreateLogger();
        }

        /// <summary>
        /// Convient to print log
        /// </summary>
        /// <param name="level"></param>
        /// <param name="error"></param>
        public static void ToLog(LogEventLevel level, string error)
        {
            switch (level)
            {
                case LogEventLevel.Verbose:
                    ServerManagerBase.LogWriter.Log.Verbose(error);
                    break;
                case LogEventLevel.Information:
                    ServerManagerBase.LogWriter.Log.Information(error);
                    break;
                case LogEventLevel.Debug:
                    ServerManagerBase.LogWriter.Log.Debug(error);
                    break;
                case LogEventLevel.Error:
                    ServerManagerBase.LogWriter.Log.Error(error);
                    break;
                case LogEventLevel.Fatal:
                    ServerManagerBase.LogWriter.Log.Fatal(error);
                    break;
                case LogEventLevel.Warning:
                    ServerManagerBase.LogWriter.Log.Warning(error);
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
