using System;
using System.IO;
using GameSpyLib.Database;
using GameSpyLib.Logging;
using RetroSpyServer.Server;

namespace RetroSpyServer
{
    /// <summary>
    /// This class represents a RetroSpy Server program
    /// </summary>
    class Program
    {
        /// <summary>
        /// Indicates the version of the server
        /// </summary>
        //public static readonly Version Version = Version.Parse(Application.ProductVersion);
        public static readonly string version = "0.1";

        public static string basePath { get; protected set; }

        /// <summary>
        /// Entry point for the RetroSpy Server program
        /// </summary>
        /// <param name="args">List of arguments passed to the application</param>
        static void Main(string[] args)
        {
            bool NoConsoleInput = false, exceptInitPathArg = false;
			string logPath = "";
			
            basePath = AppDomain.CurrentDomain.BaseDirectory;

            // Argument switcher
            foreach (var argument in args)
            {
                if (exceptInitPathArg)
                {
                    basePath = argument;
                    exceptInitPathArg = false;
                }
                else if (argument == "--help")
                {
                    Console.WriteLine("List of arguments avaiable:\n" +
                        "--help\tPrints this screen\n" +
                        "--no-cli-input\tDisables console input, usefull for services\n" +
                        "--init-path [path]\tUse a custom base path for load the configuration and save the logs"
                    );

                    // Close the program after --help
                    return;
                }
                else if (argument == "--no-cli-input")
                    NoConsoleInput = true;
                else if (argument == "--init-path")
                    exceptInitPathArg = true;
                else
                {
                    Console.WriteLine("Unknown argument {0}", argument);
                }
            }

            if (exceptInitPathArg)
            {
                Console.WriteLine("The argument \"--init-path\" requires an argument!");
                return;
            }

            //string version = String.Concat(Version.Major, ".", Version.Minor, ".", Version.Build);

            Console.Title = "RetroSpy Server " + version;

            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            logPath = basePath + @"/Logs/";

            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);

            LogWriter.Log = new LogWriter(String.Format(Path.Combine(logPath, "{0}.log"), DateTime.Now.ToLongDateString()));

            Console.WriteLine(@"  ___     _           ___             ___                      ");
            Console.WriteLine(@" | _ \___| |_ _ _ ___/ __|_ __ _  _  / __| ___ _ ___ _____ _ _ ");
            Console.WriteLine(@" |   / -_)  _| '_/ _ \__ \ '_ \ || | \__ \/ -_) '_\ V / -_) '_|");
            Console.WriteLine(@" |_|_\___|\__|_| \___/___/ .__/\_, | |___/\___|_|  \_/\___|_|  ");
            Console.WriteLine(@"                         |_|   |__/                            ");
            Console.WriteLine("");

            LogWriter.Log.Write("RetroSpy Server version " + version + ".", LogLevel.Information);

            ServerFactory Emulator = null;

            try
            {
                XMLConfiguration.LoadConfiguration();

                Emulator = new ServerFactory();

                // Decide which database you want
                if (XMLConfiguration.DatabaseType == DatabaseEngine.Sqlite)
                {
                    Emulator.Create(DatabaseEngine.Sqlite, "Data Source=" + XMLConfiguration.DatabaseName + ";Version=3;New=False");
                }
                else
                {
                    Emulator.Create(DatabaseEngine.Mysql, String.Format("Server={0};Database={1};Uid={2};Pwd={3};Port={4}", XMLConfiguration.DatabaseHost, XMLConfiguration.DatabaseName, XMLConfiguration.DatabaseUsername, XMLConfiguration.DatabasePassword, XMLConfiguration.DatabasePort));
                }

                Emulator.StartServer("GPSP", 29901);
                Emulator.StartServer("GPCM", 29900);

                LogWriter.Log.Write("Server successfully started! Type \"help\" for a list of the available commands.", LogLevel.Information);

                while (Emulator.IsRunning())
                {
                    // Process console commands
                    if (!NoConsoleInput)
                    {                        
                        string input = Console.ReadLine();

                        if (input.Equals("exit"))
                            Emulator.StopAllServers();
                        else
                            Console.WriteLine("Unknown command!");
                    }
                }
            }
            catch (Exception e)
            {
                LogWriter.Log.WriteException(e);
            }

            LogWriter.Log.Write("Goodbye!", LogLevel.Information);
            Emulator.Dispose();
            LogWriter.Log.Dispose();

            // Pauses the screen
            if (!NoConsoleInput)
            {
                Console.WriteLine("Press ENTER to exit...");
                Console.ReadKey();
            }
        }
    }
}
