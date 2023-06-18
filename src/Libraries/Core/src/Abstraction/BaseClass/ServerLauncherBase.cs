using System;
using ConsoleTables;
using Figgle;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Config;
using UniSpy.Server.Core.Database.DatabaseModel;
using System.Collections.Generic;

namespace UniSpy.Server.Core.Abstraction.BaseClass.Factory
{
    /// <summary>
    /// Each server launcher only launch an instance of IServer
    /// </summary>
    public abstract class ServerLauncherBase
    {
        /// <summary>
        /// UniSpy server version
        /// </summary>
        public static readonly string Version = "0.8.1";

        public static List<IServer> ServerInstances { get; protected set; } = new List<IServer>();
        public ServerLauncherBase()
        {
        }
        public virtual void Start()
        {
            ShowUniSpyLogo();
            ConnectMySql();
            ConnectRedis();
            LaunchServer();
        }
        protected abstract List<IServer> LaunchNetworkService();
        protected virtual void LaunchServer()
        {
            ServerInstances = LaunchNetworkService();
            if (ServerInstances.Count == 0)
            {
                throw new Exception("Server created failed");
            }
            var table = new ConsoleTable("Server Name", "Listening Address", "Listening Port");
            Console.Title = $"UniSpyServer  {Version} - {ServerInstances[0].Name}";

            foreach (var server in ServerInstances)
            {
                server.Start();
                table.AddRow(server.Name, server.ListeningIPEndPoint.Address, server.ListeningIPEndPoint.Port);
            }
            table.Write(ConsoleTables.Format.Alternative);
            Console.WriteLine("Server successfully started!");
        }

        protected void ConnectRedis()
        {
            var redisConfig = ConfigManager.Config.Redis;
            try
            {
                using var r = StackExchange.Redis.ConnectionMultiplexer.Connect(redisConfig.ConnectionString);
            }
            catch (System.Exception e)
            {
                throw new UniSpy.Exception("Can not connect to Redis", e);
            }
            Console.WriteLine($"Successfully connected to Redis at {redisConfig.Server}:{redisConfig.Port}");
        }
        protected void ConnectMySql()
        {
            //Determine which database is used and establish the database connection.
            var dbConfig = ConfigManager.Config.Database;
            if (!new UniSpyContext().Database.CanConnect())
            {
                throw new Exception($"Can not connect to {dbConfig.Type}!");
            }

            Console.WriteLine($"Successfully connected to {dbConfig.Type} at {dbConfig.Server}:{dbConfig.Port}");
        }
        protected static void ShowUniSpyLogo()
        {
            Console.WriteLine("");
            Console.WriteLine(FiggleFonts.Small.Render("UniSpy.Server"));
            Console.WriteLine(@"Version: " + Version);
        }
    }
}