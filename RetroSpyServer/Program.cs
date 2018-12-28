using System;
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

        /// <summary>
        /// Entry point for the RetroSpy Server program
        /// </summary>
        /// <param name="args">List of arguments passed to the application</param>
        static void Main(string[] args)
        {
            Console.Title = "RetroSpy Server";

            //string version = String.Concat(Version.Major, ".", Version.Minor, ".", Version.Build);
            string version = "0.1";

            Logger.Create(Environment.CurrentDirectory);

            Console.WriteLine(@"  ___     _           ___             ___                      ");
            Console.WriteLine(@" | _ \___| |_ _ _ ___/ __|_ __ _  _  / __| ___ _ ___ _____ _ _ ");
            Console.WriteLine(@" |   / -_)  _| '_/ _ \__ \ '_ \ || | \__ \/ -_) '_\ V / -_) '_|");
            Console.WriteLine(@" |_|_\___|\__|_| \___/___/ .__/\_, | |___/\___|_|  \_/\___|_|  ");
            Console.WriteLine(@"                         |_|   |__/                            ");
            Console.WriteLine("");

            Logger.Info("RetroSpy Server version " + version + ".");

            ServerFactory Emulator = new ServerFactory();

            try
            {
                Emulator.Create(DatabaseEngine.Sqlite, "Data Source=:memory:;Version=3;New=True");

                Logger.Info("Starting Presence Search Player Server at 127.0.0.1:29901..."); // TODO: Add config!
                Emulator.StartServer("GPSP", "127.0.0.1", 29901, 100);

                Logger.Info("Server successfully started! Type \"help\" for a list of the avaiable commands.");

                while (Emulator.IsRunning())
                {
                    Console.Write("command>");

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
                Logger.Fatal(e);
            }

            Logger.Info("Goodbye!");
            Emulator.Dispose();
            Logger.Dispose();
        }
    }
}
