using System;
using System.IO;
using GameSpyLib;
using GameSpyLib.Database;

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

            LogWriter.Create(path);

            Console.WriteLine(@"  ___     _           ___             ___                      ");
            Console.WriteLine(@" | _ \___| |_ _ _ ___/ __|_ __ _  _  / __| ___ _ ___ _____ _ _ ");
            Console.WriteLine(@" |   / -_)  _| '_/ _ \__ \ '_ \ || | \__ \/ -_) '_\ V / -_) '_|");
            Console.WriteLine(@" |_|_\___|\__|_| \___/___/ .__/\_, | |___/\___|_|  \_/\___|_|  ");
            Console.WriteLine(@"                         |_|   |__/                            ");
            Console.WriteLine("");

            LogWriter.Info("RetroSpy Server version " + version + ".");

            ServerFactory Emulator = new ServerFactory();

            try
            {
                //deside which database you want;
                Emulator.Create(DatabaseEngine.Sqlite, "Data Source=:memory:;Version=3;New=True");

                LogWriter.Info("Starting Presence Search Player Server at 127.0.0.1:29901..."); // TODO: Add config!
                Emulator.StartServer("GPSP", "127.0.0.1", 29901, 100);

                LogWriter.Info("Server successfully started! Type \"help\" for a list of the avaiable commands.");

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
                LogWriter.Fatal(e);
            }

            LogWriter.Info("Goodbye!");
            Emulator.Dispose();
            LogWriter.Dispose();
        }
    }
}
