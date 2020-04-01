using GameSpyLib.Logging;
using System;

namespace Chat.Application
{
    /// <summary>
    /// This class represents a RetroSpy Server program
    /// </summary>
    internal class Program
    {
        public static readonly string ServerName = "CHAT";

        public static string BasePath { get; protected set; }

        //public static bool IsWindows() => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        private static ServerManager Manager = null;

        public static bool IsRunning = false;

        /// <summary>
        /// Entry point for the RetroSpy Server program
        /// </summary>
        /// <param name="args">List of arguments passed to the application</param>
        ///<param name="bool_InitPathArg">argument for Main()</param>
        private static void Main(string[] args)
        {
            //if (IsWindows()) { Console.WindowWidth = 100; } // Temp fix for Linux and MacOS?

            //you can choose whether accept command input.
            bool IsConsoleInputAvailable = false;

            // Whether accept  args input.
            bool IsInitPathArgAvailable = false;

            BasePath = AppDomain.CurrentDomain.BaseDirectory;

            Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs e)
            {
                e.Cancel = true;
                IsRunning = false;
            };

            #region Argument Setting
            // Argument switcher
            foreach (var argument in args)
            {
                if (IsInitPathArgAvailable)
                {
                    BasePath = argument;
                    IsInitPathArgAvailable = false;
                }
                else if (argument == "--help")
                {
                    Console.WriteLine("List of arguments available:\n" +
                        "--help\tPrints this screen\n" +
                        "--no-cli-input\tDisables console input, usefull for services\n" +
                        "--init-path [path]\tUse a custom base path for load the configuration and save the logs"
                    );

                    // Close the program after --help
                    return;
                }
                else if (argument == "--no-cli-input")
                {
                    IsConsoleInputAvailable = false;
                }
                else if (argument == "--init-path")
                {
                    IsInitPathArgAvailable = true;
                }
                else
                {
                    Console.WriteLine("Unknown argument {0}", argument);
                }
            }

            if (IsInitPathArgAvailable)
            {
                Console.WriteLine("The argument \"--init-path\" requires an argument!");
                return;
            }
            #endregion

            try
            {
                //create a instance of ServerManager class
                Manager = new ServerManager(ServerName);
                Console.Title = "RetroSpy Server " + Manager.RetroSpyVersion;
                IsRunning = true;
            }
            catch (Exception e)
            {
                LogWriter.Log.WriteException(e);
            }

            if (IsConsoleInputAvailable)
            {
                LogWriter.Log.Write("Server is successfully started! \nType \"help\" for a list of the available commands.", LogLevel.Info);
                //Read key from console
                while (IsRunning)
                {
                    // Process console commands
                    //if (bool_ConsoleInput)
                    //{
                    string input = Console.ReadLine();
                    switch (input)
                    {
                        case "exit":
                            Manager.Dispose();
                            IsRunning = false;
                            break;

                        case "help":
                            Console.WriteLine("--exit \t shutdown all servers and exit the RetroSpy emulator\n" +
                                                           "other features are comming soon..\n");
                            break;

                        default:
                            Console.WriteLine("Unknown command!");
                            break;
                    }
                }
            }
            else
            {
                // Pauses the screen                
                Console.WriteLine("Press ENTER to exit...");
                Console.ReadLine();
            }
            #region Program Dispose
            LogWriter.Log.Write("Goodbye!", LogLevel.Info);
            Manager?.Dispose();
            LogWriter.Log.Dispose();
            #endregion
        }
    }
}
