using System;
using System.IO;
using System.Text;
using System.Timers;

namespace GameSpyLib.Logging
{
    public enum LogLevel : int
    {
        Debug,
        Info,
        Warning,
        Error,
        Fatal
    };

    /// <summary>
    /// Provides an object wrapper for a file that is used to
    /// store LogMessage's into. Uses a Multi-Thread safe Queueing
    /// system, and provides full Asynchronous writing and flushing
    /// </summary>
    public  class LogWriter : IDisposable
    {
        /// <summary>
        /// Public instance of the LogWriter
        /// </summary>
        public static LogWriter Log = null;

        /// <summary>
        /// Minimum logging level that will be wrote
        /// </summary>
        //public LogLevel MiniumLogLevel = LogLevel.Info;
        public LogLevel MiniumLogLevel = LogLevel.Info;
        /// <summary>
        /// If this variable is setted to true, anything the server will send and receive will be logged
        /// </summary>
        public bool DebugSockets = false;

        /// <summary>
        /// Full path to the log file
        /// </summary>
        private FileInfo LogFile;

        /// <summary>
        /// The <see cref="StreamWriter"/> for  <paramref name="LogFile"/>
        /// </summary>
        private StreamWriter LogStream;

        /// <summary>
        /// Our midnight timer, to truncate the log file
        /// </summary>
        private Timer TruncateTimer;

        /// <summary>
        /// Our lock object, preventing race conditions
        /// </summary>
        private object _sync = new object();

        /// <summary>
        /// Provides a full sync lock between all instances of this app
        /// </summary>
        private static object _fullSync = new object();

        /// <summary>
        /// Creates a new Log Writer instance
        /// </summary>
        /// <param name="FileLocation">The location of the logfile. If the file doesnt exist,
        /// It will be created.</param>
        /// <param name="Truncate">If set to true and the logfile is over XX size, it will be truncated to 0 length</param>
        /// <param name="TruncateLen">
        ///     If <paramref name="Truncate"/> is true, The size of the file must be at least this size, 
        ///     in bytes, to truncate it
        /// </param>
        public LogWriter(string FileLocation, bool Truncate = false, int TruncateLen = 2097152)
        {
            // Test that we are able to open and write to the file
            LogFile = new FileInfo(FileLocation);
            FileStream fileStream = LogFile.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            LogStream = new StreamWriter(fileStream, Encoding.UTF8);

            // If the file is over 2MB, and we want to truncate big files
            if (Truncate && LogFile.Length > TruncateLen)
            {
                LogStream.BaseStream.SetLength(0);
                LogStream.BaseStream.Seek(0, SeekOrigin.Begin);
                LogStream.Flush();
            }
            else if (LogFile.Length > 0)
            {
                // Seek to the end of the log file
                LogStream.BaseStream.Seek(0, SeekOrigin.End);
            }

            // 24 Truncate Timer
            if (Truncate)
            {
                // Create Truncation Task
                TimeSpan untilMidnight = DateTime.Today.AddDays(1) - DateTime.Now;
                TruncateTimer = new Timer();
                TruncateTimer.AutoReset = false; // Don't reset first time around!
                TruncateTimer.Interval = untilMidnight.TotalMilliseconds;
                TruncateTimer.Elapsed += TruncateTimer_Elapsed;
                TruncateTimer.Start();
            }
        }

        /// <summary>
        /// Event called at midnight, to clear the log file
        /// </summary>
        private void TruncateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                // Get next 24 hours
                TruncateTimer.Interval = 24 * 60 * 60 * 1000;
                TruncateTimer.Enabled = true;
                TruncateTimer.Start();

                // Only allow 1 thread at a time do these operations
                lock (_sync)
                {
                    // Empty log
                    LogStream.BaseStream.SetLength(0);
                    LogStream.BaseStream.Seek(0, SeekOrigin.Begin);
                    LogStream.Flush();
                }

                // Write to console
                lock (_fullSync)
                {
                    // TODO! Rotate the file
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.WriteLine("Clearing logfile: " + LogFile.Name);
                    Console.WriteLine();
                }
            }
            catch (Exception E)
            {
                Console.WriteLine(E.Message);
            }
        }

        /// <summary>
        /// Adds a message to the queue, to be written to the log file
        /// </summary>
        /// <param name="message">The message to write to the log</param>
        /// <param name="level">The level of the log</param>
        public void Write(string message, LogLevel level)
        {
            if (level < MiniumLogLevel)
                return;

            // Only allow 1 thread at a time do these operations
            lock (_sync)
            {
                Console.WriteLine(string.Format("[{0}] [{2}]\t{1}", DateTime.Now, message, level.ToString()));
                LogStream.WriteLine(string.Format("[{0}] [{2}]\t{1}", DateTime.Now, message, level.ToString()));
                LogStream.Flush();
            }
        }

        /// <summary>
        /// Adds a message to the queue, to be written to the log file
        /// </summary>
        /// <param name="message">The message to write to the log</param>
        /// <param name="level">The level of the log</param>
        /// <param name="items">Extra items to be appended to the message</param>
        public void Write(string message, LogLevel level, params object[] items)
        {
            if (level < MiniumLogLevel)
                return;

            // Only allow 1 thread at a time do these operations
            lock (_sync)
            {
                Console.WriteLine(string.Format("[{0}] [{2}]\t{1}", DateTime.Now, string.Format(message, items), level.ToString()));
                LogStream.WriteLine(string.Format("[{0}] [{2}]\t{1}", DateTime.Now, string.Format(message, items), level.ToString()));
                LogStream.Flush();
            }
        }

        /// <summary>
        /// Writes an Exception to the log file
        /// </summary>
        /// <param name="ex">The exception to write</param>
        public void WriteException(Exception ex)
        {
            Write("Exception {0} (HRESULT: {1})\n{2}", LogLevel.Error, ex.Message, ex.HResult, ex.StackTrace);
        }

        /// <summary>
        /// Destructor. Make sure we flush!
        /// </summary>
        ~LogWriter()
        {
            Dispose();
        }
        /// <summary>
        /// free resource created by this class
        /// </summary>
        public void Dispose()
        {
            LogStream.Close();

            try
            {
                LogStream?.Dispose();
                TruncateTimer?.Dispose();
            }
            catch (ObjectDisposedException) { } // Ignore
        }
    }
}
