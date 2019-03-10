using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using GameSpyLib.Log;
using GameSpyLib.Database;

namespace RetroSpyServer
{
    /// <summary>
    /// This class represents the configuration of a server
    /// </summary>
    public class ServerConfiguration
    {
        /// <summary>
        /// The IP that the server will bind
        /// </summary>
        public string ip = "localhost";

        /// <summary>
        /// The port that the server will use
        /// </summary>
        public int port = 0;

        /// <summary>
        /// Max connections available for the server
        /// </summary>
        public int maxConnections = 0;
    };

    /// <summary>
    /// This class represents an XML configuration parser and saver
    /// </summary>
    public class XMLConfiguration
    {
        /// <summary>
        /// Contains the database name
        /// </summary>
        public static string DatabaseName { get; protected set; }

        /// <summary>
        /// Contains the IP of the database
        /// </summary>
        public static string DatabaseHost { get; protected set; }

        /// <summary>
        /// Contains the username that will be used to login in the database
        /// </summary>
        public static string DatabaseUsername { get; protected set; }

        /// <summary>
        /// Contains the password that will be used to login in the database
        /// </summary>
        public static string DatabasePassword { get; protected set; }

        /// <summary>
        /// The type of the database that will be used
        /// </summary>
        public static DatabaseEngine DatabaseType { get; protected set; }

        /// <summary>
        /// The port of the database the server will connect
        /// </summary>
        public static int DatabasePort { get; protected set; }

        /// <summary>
        /// A default IP if no server IP is setted
        /// </summary>
        public static string DefaultIP { get; protected set; }

        /// <summary>
        /// Sets the max connection if it is not specified for the server
        /// </summary>
        public static int DefaultMaxConnections { get; protected set; }

        public static Dictionary<string, ServerConfiguration> ServerConfig { get; protected set; }

        protected static void CreateBlankConfiguration()
        {
            try
            {
                StreamWriter file = new StreamWriter(Path.Combine(Program.basePath, "RetroSpyServer.xml"));

                file.Write("<?xml version=\"1.0\"?>\n" +
                    "<Configuration>\n" +
                    "\t<Database type=\"sqlite\"\n>" +
                    "\t\t<Name>sqlite_retrospy_server.db</Name> <!--In Sqlite is the Path, in MySQL is the database name -->\n" +
                    "\t\t<!-- MySQL only -->\n" +
                    "\t\t<Host></Host>\n" +
                    "\t\t<Username>retrospy</Username>\n" +
                    "\t\t<Password></Password>\n" +
                    "\t</Database>\n\n" +
                    "\t<DefaultIP>127.0.0.1</DefaultIP>\n" +
                    "</Configuration>\n");

                file.Close();
            }
            catch (Exception ex)
            {
                LogWriter.Log.WriteException(ex);
            }
        }

        public static void LoadConfiguration()
        {
            ServerConfig = new Dictionary<string, ServerConfiguration>();

            // Default values
            DatabaseType = DatabaseEngine.Sqlite;
            DatabaseName = ":memory:";
            DatabaseHost = DatabaseUsername = DatabasePassword = "";
            DatabasePort = 3306;
            DefaultIP = "localhost";
            DefaultMaxConnections = 100;

            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(Path.Combine(Program.basePath, "RetroSpyServer.xml"));
            }
            catch (Exception ex)
            {
                LogWriter.Log.WriteException(ex);
                LogWriter.Log.Write("Creating configuration file...", LogLevel.Info);

                CreateBlankConfiguration();
                return;
            }

            if (doc.ChildNodes.Count < 2 || doc.ChildNodes[0].Name != "xml" || doc.ChildNodes[1].Name != "Configuration")
            {
                LogWriter.Log.Write("Invalid configuration file! Missing <Configuration> and <xml> tag!", LogLevel.Error);
                return;
            }
            
            foreach (XmlNode node in doc.ChildNodes[1].ChildNodes)
            {
                if (node.Name == "Database")
                {
                    foreach (XmlAttribute attr in node.Attributes)
                    {
                        if (attr.Name == "type")
                        {
                            switch (attr.InnerText.ToLower())
                            {
                                case "mysql":
                                    DatabaseType = DatabaseEngine.Mysql;
                                    break;
                                case "sqlite":
                                    DatabaseType = DatabaseEngine.Sqlite;
                                    break;
                                default:
                                    LogWriter.Log.Write("Unknown database engine " + attr.InnerText + "! Defaulting Sqlite...", LogLevel.Warning);
                                    break;
                            }
                        }
                    }

                    foreach (XmlNode node2 in node.ChildNodes)
                    {
                        if (node2.Name == "Name")
                            DatabaseName = node2.InnerText;

                        else if (node2.Name == "Username")
                            DatabaseUsername = node2.InnerText;

                        else if (node2.Name == "Password")
                            DatabasePassword = node2.InnerText;

                        else if (node2.Name == "Host")
                            DatabaseHost = node2.InnerText;

                        else if (node2.Name == "Port")
                        {
                            // Try to parse the database port, if it fails set it
                            // to default port 3306
                            int port = 0;
                            if (!int.TryParse(node2.InnerText, out port) || port < 1)
                                port = 3306;

                            DatabasePort = port;
                        }
                    }
                }

                else if (node.Name == "DefaultIP")
                {
                    DefaultIP = node.InnerText;
                }

                else if (node.Name == "DefaultMaxConnections")
                {
                    int max = 0;
                    if (!int.TryParse(node.InnerText, out max) || max < 1)
                    {
                        LogWriter.Log.Write("Unable to read default max connections! Defaulting to 100...", LogLevel.Warning);
                        max = 100;
                    }

                    DefaultMaxConnections = max;
                }

                else if (node.Name == "LogLevel")
                {
                    if (!Enum.TryParse<LogLevel>(node.InnerText, out LogWriter.Log.MiniumLogLevel))
                    {
                        LogWriter.Log.Write("Unable to set LogLevel! Defaulting to Information...", LogLevel.Warning);
                        LogWriter.Log.MiniumLogLevel = LogLevel.Info;
                    }
                }

                else if (node.Name == "DebugSocket")
                {
                    if (node.InnerText.Equals("true"))
                    {
                        LogWriter.Log.DebugSockets = true;
                        LogWriter.Log.Write("Socket debugging is enabled!", LogLevel.Debug);
                    }
                }

                else if (node.Name == "Server")
                {
                    string realServerName = "";

                    foreach (XmlAttribute attr in node.Attributes)
                    {
                        if (attr.Name == "name")
                            realServerName = attr.InnerText.ToUpper();
                    }

                    if (realServerName.Length < 1)
                        continue;

                    if (!ServerConfig.ContainsKey(realServerName))
                        ServerConfig.Add(realServerName, new ServerConfiguration());

                    foreach (XmlNode node2 in node.ChildNodes)
                    {
                        if (node2.Name == "IP")
                        {
                            ServerConfig[realServerName].ip = node2.InnerText;
                        }

                        else if (node2.Name == "Port")
                        {
                            if (!int.TryParse(node2.InnerText, out ServerConfig[realServerName].port) || ServerConfig[realServerName].port < 1)
                                ServerConfig[realServerName].port = -1;
                        }

                        else if (node2.Name == "MaxConnections")
                        {
                            if (!int.TryParse(node2.InnerText, out ServerConfig[realServerName].maxConnections) || ServerConfig[realServerName].maxConnections < 1)
                                ServerConfig[realServerName].maxConnections = -1;
                        }
                    }
                }
            }
        }
    }
}
