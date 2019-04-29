using System;
using System.Xml.Serialization;
using System.IO;
using RetroSpyServer.Config;

namespace RetroSpyServer.Application
{
    /// <summary>
    /// This class represents an XML configuration parser and saver
    /// </summary>
    public class ConfigManager
    {
        private static XMLConfigurationArttributes XmlConfiguration = null;

        private static ServerConfigurationArttributes DefaultServerConfiguration = null;


        public static XMLConfigurationArttributes GetXMLConfiguration()
        {
            return XmlConfiguration;
        }

        public static bool Load()
        {
            FileStream stream = new FileStream(Path.Combine(Program.basePath, "RetroSpyServer.xml"), FileMode.OpenOrCreate);

            XmlSerializer serializer = new XmlSerializer(typeof(XMLConfigurationArttributes));
            XmlConfiguration = (XMLConfigurationArttributes)serializer.Deserialize(stream);
            stream.Close();

            DefaultServerConfiguration = new ServerConfigurationArttributes
            {
                Name = "Default",
                Disabled = false,
                MaxConnections = 1000,
                Port = 0,
                Hostname = XmlConfiguration.BindIP,
            };

            return true;
        }

        public static bool Save()
        {
            FileStream stream = new FileStream(Path.Combine(Program.basePath, "RetroSpyServer.xml"), FileMode.Create);

            XmlSerializer serializer = new XmlSerializer(typeof(XMLConfigurationArttributes));
            serializer.Serialize(stream, XmlConfiguration);
            stream.Close();

            return true;
        }

        public static ServerConfigurationArttributes GetServerConfiguration(string name)
        {
            if (XmlConfiguration.Servers == null)
                return DefaultServerConfiguration;

            //Check which servers are in the XML file
            foreach (ServerConfigurationArttributes cfg in XmlConfiguration.Servers)
            {
                if (cfg.Name.Equals(name.ToUpper()))
                    return cfg;
            }

            return DefaultServerConfiguration;
        }
    }
}
