using System;
using System.IO;
using System.Xml.Serialization;

namespace GameSpyLib.XMLConfig
{
    /// <summary>
    /// This class represents an XML configuration parser and saver
    /// </summary>
    public class ConfigManager
    {
        public static XMLConfiguration xmlConfig { get; protected set; }

        public static bool Load()
        {
            // Load XML file
            {

                FileStream fstream = File.OpenRead(@"RetroSpyServer.xml");
                //FileStream fstream = new FileStream(fullpath, FileMode.Open,FileAccess.Read,FileShare.Read);

                fstream.Seek(0, SeekOrigin.Begin);
                //stream.Position = 0;
                XmlSerializer serializer = new XmlSerializer(typeof(XMLConfiguration));
                xmlConfig = (XMLConfiguration)serializer.Deserialize(fstream);
                fstream.Close();
            }

            // Perform XML validation
            {
                if (xmlConfig.Database == null)
                {
                    throw new Exception("Database configuration not specified!");
                }

                if (xmlConfig.Database.Type == Database.Entity.DatabaseEngine.MySql)
                {
                    if (xmlConfig.Database.Username.Length < 1 ||
                        xmlConfig.Database.Hostname.Length < 1 ||
                        xmlConfig.Database.Databasename.Length < 1 ||
                        xmlConfig.Database.SslMode.Length < 1)
                    {
                        throw new Exception("Invalid database configuration!");
                    }

                    if (xmlConfig.Database.Port < 1)
                        xmlConfig.Database.Port = 3306;
                }
                //issue if servers config not exsit this will not throw an exception
                if (xmlConfig.Servers == null || xmlConfig.Servers.Length < 1)
                {
                    throw new Exception("Server configuration not specified!");
                }

                if (xmlConfig.Redis == null || xmlConfig.Redis.Hostname.Length < 1)
                {
                    throw new Exception("Redis configuration not specified!");
                }

                foreach (ServerConfiguration servercfg in xmlConfig.Servers)
                {

                    if (servercfg.Hostname.Length < 1 ||
                        servercfg.Port < 1 ||
                        servercfg.Name.Length < 1)
                        throw new Exception(string.Format("Invalid {0} configuration", servercfg.Name));

                    servercfg.Name = servercfg.Name.ToUpperInvariant();
                }
            }

            return true;
        }
    }
}
