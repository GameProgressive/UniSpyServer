using System;
using System.Xml.Serialization;
using System.IO;

namespace RetroSpyServer.XMLConfig
{
    /// <summary>
    /// This class represents an XML configuration parser and saver
    /// </summary>
    public class ConfigManager
    {
        public static XMLConfiguration Configuration { get; protected set; }

        public static bool Load()
        {
            // Load XML file
            {
                FileStream stream = new FileStream(Path.Combine(Program.BasePath, "RetroSpyServer.xml"), FileMode.OpenOrCreate);
                stream.Seek(0, SeekOrigin.Begin);

                XmlSerializer serializer = new XmlSerializer(typeof(XMLConfiguration));
                Configuration = (XMLConfiguration)serializer.Deserialize(stream);
                stream.Close();
            }

            // Perform XML validation
            {
                if (Configuration.Database == null)
                {
                    throw new Exception("Database configuration not specified!");
                }

                if (Configuration.Database.Type == GameSpyLib.Database.DatabaseEngine.Mysql)
                {
                    if (Configuration.Database.Username.Length < 1 || 
                        Configuration.Database.Hostname.Length < 1 || Configuration.Database.Name.Length < 1)
                    {
                        throw new Exception("Invalid database configuration!");
                    }

                    if (Configuration.Database.Port < 1)
                        Configuration.Database.Port = 3306;
                }

                if (Configuration.Servers == null)
                {
                    throw new Exception("Server configuration not specified!");
                }

                foreach (ServerConfiguration config in Configuration.Servers)
                {
                    if (config.Hostname.Length < 1 || config.MaxConnections < 1 || config.Port < 1 || config.Name.Length < 1)
                        throw new Exception(string.Format("Invalid {0} configuration", config.Name));

                    config.Name = config.Name.ToUpperInvariant();
                }
            }

            return true;
        }
    }
}
