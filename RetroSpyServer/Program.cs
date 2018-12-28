using System;
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
        private static readonly string ServerVersion = "0.1";

        /// <summary>
        /// Entry point for the RetroSpy Server program
        /// </summary>
        /// <param name="args">List of arguments passed to the application</param>
        static void Main(string[] args)
        {
            Console.WriteLine(@"  ___     _           ___             ___                      ");
            Console.WriteLine(@" | _ \___| |_ _ _ ___/ __|_ __ _  _  / __| ___ _ ___ _____ _ _ ");
            Console.WriteLine(@" |   / -_)  _| '_/ _ \__ \ '_ \ || | \__ \/ -_) '_\ V / -_) '_|");
            Console.WriteLine(@" |_|_\___|\__|_| \___/___/ .__/\_, | |___/\___|_|  \_/\___|_|  ");
            Console.WriteLine(@"                         |_|   |__/                            ");

            Console.WriteLine("\nRetroSpy Server version {0}.", ServerVersion);

            ServerFactory Emulator = new ServerFactory();

            try
            {
                Emulator.Create(DatabaseEngine.Sqlite, "Data Source=:memory:;Version=3;New=True");

                Console.WriteLine("Starting GPSP at 127.0.0.1:29901...");
                Emulator.StartServer("GPSP", "127.0.0.1", 29901, 100);

                Console.WriteLine("Server started!");

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
                Console.WriteLine(e.ToString());
            }

            Emulator.Dispose();
        }
    }
}
