using System;
using System.Xml;
using GameSpyLib.Logging;
using GameSpyLib.Database;

namespace RetroSpyServer
{
    public class XMLConfiguration
    {
        public static string DatabaseName { get; protected set; }
        public static string DatabaseHost { get; protected set; }
        public static string DatabaseUsername { get; protected set; }
        public static string DatabasePassword { get; protected set; }
        public static DatabaseEngine DatabaseType { get; protected set; }
        public static int DatabasePort { get; protected set; }
        public static string DefaultIP { get; protected set; }

        public static void LoadConfiguration()
        {
            // Default values
            DatabaseType = DatabaseEngine.Sqlite;
            DatabaseName = ":memory:";
            DatabaseHost = DatabaseUsername = DatabasePassword = "";
            DatabasePort = 3306;
            DefaultIP = "localhost";

            XmlDocument doc = new XmlDocument();
            doc.Load("RetroSpyServer.xml");

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
                            switch (attr.InnerText)
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
                            if (!int.TryParse(node2.InnerText, out port))
                                port = 3306;

                            DatabasePort = port;
                        }
                    }
                }

                else if (node.Name == "DefaultIP")
                {
                    DefaultIP = node.InnerText;
                }

                else if (node.Name == "LogLevel")
                {
                    if (!Enum.TryParse<LogLevel>(node.InnerText, out LogWriter.Log.MiniumLogLevel))
                    {
                        LogWriter.Log.Write("Unable to set LogLevel! Defaulting to Information...", LogLevel.Warning);
                        LogWriter.Log.MiniumLogLevel = LogLevel.Information;
                    }
                }

                else if (node.Name == "DebugSocket")
                {
                    if (node.InnerText.Equals("true"))
                    {
                        LogWriter.Log.DebugSockets = true;
                        LogWriter.Log.Write("Socket debugging is enabled!", LogLevel.Information);
                    }
                }
            }
        }
    }
}
