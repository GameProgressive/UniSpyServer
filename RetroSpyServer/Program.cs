using System;
using System.IO;
using GameSpyLib;
using GameSpyLib.Database;
using GameSpyLib.Logging;

namespace RetroSpyServer
{
    /// <summary>
    /// This class rapresents a RetroSpy Server program
    /// </summary>
    class Program
    {
        /// <summary>
        /// Indicates the version of the server
        /// </summary>
        //public static readonly Version Version = Version.Parse(Application.ProductVersion);
        public static readonly string version = "0.1";

        /// <summary>
        /// Entry point for the RetroSpy Server program
        /// </summary>
        /// <param name="args">List of arguments passed to the application</param>
        static void Main(string[] args)
        {
            //string version = String.Concat(Version.Major, ".", Version.Minor, ".", Version.Build);
            string path = Path.Combine(Environment.CurrentDirectory, "Logs");

            Console.Title = "RetroSpy Server " + version;

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            LogWriter.Log = new LogWriter(String.Format(path + "{0}.log", DateTime.Now.ToLongDateString()));

            Console.WriteLine(@"  ___     _           ___             ___                      ");
            Console.WriteLine(@" | _ \___| |_ _ _ ___/ __|_ __ _  _  / __| ___ _ ___ _____ _ _ ");
            Console.WriteLine(@" |   / -_)  _| '_/ _ \__ \ '_ \ || | \__ \/ -_) '_\ V / -_) '_|");
            Console.WriteLine(@" |_|_\___|\__|_| \___/___/ .__/\_, | |___/\___|_|  \_/\___|_|  ");
            Console.WriteLine(@"                         |_|   |__/                            ");
            Console.WriteLine("");

            LogWriter.Log.Write("RetroSpy Server version " + version + ".", LogLevel.Information);

            XMLConfiguration.LoadConfiguration();

            ServerFactory Emulator = new ServerFactory();

            try
            {
                // Decide which database you want
                if (XMLConfiguration.DatabaseType == DatabaseEngine.Sqlite)
                {
                    Emulator.Create(DatabaseEngine.Sqlite, "Data Source=" + XMLConfiguration.DatabaseName + ";Version=3;New=False");
                }
                else
                {
                    Emulator.Create(DatabaseEngine.Mysql, String.Format("Server={0};Database={1};Uid={2};Pwd={3};Port={4}", XMLConfiguration.DatabaseHost, XMLConfiguration.DatabaseUsername, XMLConfiguration.DatabaseUsername, XMLConfiguration.DatabasePassword, XMLConfiguration.DatabasePort));
                }

                Emulator.StartServer("GPSP", 29901, 100);

                LogWriter.Log.Write("Server successfully started! Type \"help\" for a list of the avaiable commands.", LogLevel.Information);

                while (Emulator.IsRunning())
                {
                    // Process console commands
                    string input = Console.ReadLine();

                    if (input.Equals("exit"))
                        Emulator.StopAllServers();
                    else
                        Console.WriteLine("Unknown command!");
                }
            }
            catch (Exception e)
            {
                LogWriter.Log.Write(e.Message, LogLevel.Fatal);
            }

            LogWriter.Log.Write("Goodbye!", LogLevel.Information);
            Emulator.Dispose();
            LogWriter.Log.Dispose();
        }
    }
}
