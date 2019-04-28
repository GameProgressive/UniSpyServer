using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using GameSpyLib.Logging;
using GameSpyLib.Database;
using RetroSpyServer.Config;

namespace RetroSpyServer.Application
{
    /// <summary>
    /// This class represents an XML configuration parser and saver
    /// </summary>
    public class Config
    {
        private static XMLConfiguration XmlConfiguration = null;

        private static ServerConfiguration DefaultServerConfiguration = null;

        public static XMLConfiguration GetXMLConfiguration()
        {
            return XmlConfiguration;
        }

        public static bool Load()
        {
            FileStream stream = new FileStream(Path.Combine(Program.basePath, "RetroSpyServer.xml"), FileMode.OpenOrCreate);

            XmlSerializer serializer = new XmlSerializer(typeof(XMLConfiguration));
            XmlConfiguration = (XMLConfiguration)serializer.Deserialize(stream);
            stream.Close();

            DefaultServerConfiguration = new ServerConfiguration
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

            XmlSerializer serializer = new XmlSerializer(typeof(XMLConfiguration));
            serializer.Serialize(stream, XmlConfiguration);
            stream.Close();

            return true;
        }

        public static ServerConfiguration GetServerConfiguration(string name)
        {
            if (XmlConfiguration.Servers == null)
                return DefaultServerConfiguration;

            foreach (ServerConfiguration cfg in XmlConfiguration.Servers)
            {
                if (cfg.Name.Equals(name.ToUpper()))
                    return cfg;
            }

            return DefaultServerConfiguration;
        }
    }
}
