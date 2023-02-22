using System;
using System.Linq;
using ConsoleTables;
using Figgle;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Config;
using UniSpy.Server.Core.Database.DatabaseModel;

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
        public static readonly string Version = "0.7.5";
        /// <summary>
        /// UniSpy server name
        /// </summary>
        /// <returns></returns>
        public static string ServerName { get; protected set; }
        public static IServer ServerInstance { get; protected set; }
        public ServerLauncherBase(string serverName)
        {
            ServerName = serverName;
        }

        public virtual void Start()
        {
            Console.Title = $"UniSpyServer  {Version} - {ServerName}";
            ShowUniSpyLogo();
            ConnectMySql();
            ConnectRedis();
            LaunchServer();
        }
        protected abstract IServer LaunchNetworkService(UniSpyServerConfig config);
        protected virtual void LaunchServer()
        {
            var cfg = ConfigManager.Config.Servers.Where(s => s.ServerName == ServerName).First();
            ServerInstance = LaunchNetworkService(cfg);

            if (ServerInstance is null)
            {
                throw new Exception("Server created failed");
            }
            // asp.net web server does not implement a Server interface, therefore this code should not be called
            ServerInstance.Start();
            var table = new ConsoleTable("Server Name", "Listening Address", "Listening Port");
            table.AddRow(ServerName, ServerInstance.ListeningIPEndPoint.Address, ServerInstance.ListeningIPEndPoint.Port);
            table.Write(ConsoleTables.Format.Alternative);
            Console.WriteLine("Server successfully started!");
        }

        protected void ConnectRedis()
        {
            var redisConfig = ConfigManager.Config.Redis;
            try
            {
                var r = StackExchange.Redis.ConnectionMultiplexer.Connect(redisConfig.ConnectionString);
                r.Dispose();
            }
            catch (Exception e)
            {
                throw new Exception("Can not connect to Redis", e);
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
