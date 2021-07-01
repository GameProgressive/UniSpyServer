using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database;
using UniSpyLib.Database.DatabaseModel.MySql;
using UniSpyLib.MiscMethod;
using UniSpyLib.UniSpyConfig;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class UniSpyServerFactoryBase
    {
        /// <summary>
        /// UniSpy server version
        /// </summary>
        public static readonly string UniSpyVersion = "0.5.3";
        public static readonly string ServerName = Assembly.GetEntryAssembly().GetName().Name;
        /// <summary>
        /// Redis connection
        /// </summary>
        public static ConnectionMultiplexer Redis { get; protected set; }
        /// <summary>
        /// A UniSpyServer instance
        /// </summary>
        public static IUniSpyServer Server { get; protected set; }
        /// <summary>
        /// The gamespy requests and UniSpy requets mapping dictionary
        /// </summary>
        public static Dictionary<object, Type> RequestMapping { get; private set; }
        /// <summary>
        /// The gamespy requests and UniSpy requets mapping dictionary
        /// </summary>
        public static Dictionary<object, Type> HandlerMapping { get; private set; }
        public UniSpyServerFactoryBase()
        {
        }

        public virtual void Start()
        {
             ShowUniSpyLogo();
             LoadUniSpyRequests();
             LoadUniSpyHandlers();
             ConnectMySql();
             ConnectRedis();
             LoadServerConfig();
             UniSpyJsonConverter.Initialize();
        }

        protected void LoadServerConfig()
        {
            //Add all servers
            foreach (UniSpyServerConfig cfg in ConfigManager.Config.Servers)
            {
                StartServer(cfg);
            }
            Server.Start();

            var table = new ConsoleTables.ConsoleTable("Server Name", "Listening Address", "Listening Port");
            table.AddRow(ServerName, Server.Endpoint.Address, Server.Endpoint.Port);
            table.Write(ConsoleTables.Format.Alternative);
            Console.WriteLine("Server successfully started!");
        }

        /// <summary>
        /// Over write the specific functions you want to start the server
        /// You can start all servers if you want
        /// </summary>
        /// <param name="cfg"></param>
        protected abstract void StartServer(UniSpyServerConfig cfg);

        protected void ConnectRedis()
        {
            try
            {
                Redis = ConnectionMultiplexer.Connect(
                    $"{ConfigManager.Config.Redis.RemoteAddress}:{ConfigManager.Config.Redis.RemotePort}");
            }
            catch (Exception e)
            {
                throw new Exception("Can not connect to Redis", e);
            }
            Console.WriteLine($"Successfully connected to Redis!");
        }
        protected void ConnectMySql()
        {
            //Determine which database is used and establish the database connection.
            switch (ConfigManager.Config.Database.Type)
            {
                case DatabaseType.MySql:
                    unispyContext.UniSpyMySqlConnStr =
                    $"Server={ConfigManager.Config.Database.RemoteAddress};"
                    + $"Database={ConfigManager.Config.Database.DatabaseName};"
                    + $"Uid={ConfigManager.Config.Database.UserName};"
                    + $"Pwd={ConfigManager.Config.Database.Password};"
                    + $"Port={ConfigManager.Config.Database.RemotePort};"
                    + $"SslMode={ConfigManager.Config.Database.SslMode};"
                    + $"SslCert={ConfigManager.Config.Database.SslCert};"
                    + $"SslKey={ConfigManager.Config.Database.SslKey};"
                    + $"SslCa={ConfigManager.Config.Database.SslCa}";
                    break;
            }

            try
            {
                new unispyContext().Database.CanConnect();
            }
            catch (Exception e)
            {
                throw new Exception($"Can not connect to {ConfigManager.Config.Database.Type}!", e);
            }

            Console.WriteLine($"Successfully connected to {ConfigManager.Config.Database.Type}!");
        }
        protected static void ShowUniSpyLogo()
        {
            // the ascii art font name is "small"
            Console.WriteLine(@" _   _      _ ___           ___ ");
            Console.WriteLine(@"| | | |_ _ (_) __|_ __ _  _/ __| ___ _ ___ _____ _ _ ");
            Console.WriteLine(@"| |_| | ' \| \__ \ '_ \ || \__ \/ -_) '_\ V / -_) '_|");
            Console.WriteLine(@" \___/|_||_|_|___/ .__/\_, |___/\___|_|  \_/\___|_|");
            Console.WriteLine(@"                 |_|   |__/ ");
            Console.WriteLine(@"Version: " + UniSpyVersion);
        }
        protected void LoadUniSpyRequests()
        {
            var requestNamespace = $"{this.GetType().Namespace.Split('.').First()}.Entity.Structure.Request";
            RequestMapping =  LoadUniSpyComponents(requestNamespace);
        }
        protected void LoadUniSpyHandlers()
        {
            var handlerNamespace = $"{this.GetType().Namespace.Split('.').First()}.Handler.CmdHandler";
            HandlerMapping =  LoadUniSpyComponents(handlerNamespace);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nspace"> namespace</param>
        /// <returns></returns>
        private static Dictionary<object, Type> LoadUniSpyComponents(string nspace)
        {
            var mapping = new Dictionary<object, Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany(t => t.GetTypes()).Where(t => t.FullName.Contains(nspace)).ToList();

            if (assemblies.Count() == 0)
            {
                throw new NotImplementedException("Components have not been implemented");
            }
            foreach (var assembly in assemblies)
            {
                var attr = (CommandAttribute)assembly.GetCustomAttributes()
                                                     .FirstOrDefault(x => x.GetType() == typeof(CommandAttribute));
                if (attr == null)
                    continue;

                if (mapping.ContainsKey(attr.Name))
                {
                    throw new ArgumentException($"Duplicate commands {attr.Name} for type {assembly.FullName}");
                }
                mapping.Add(attr.Name, assembly);
            }
            return mapping;
        }
    }
}
