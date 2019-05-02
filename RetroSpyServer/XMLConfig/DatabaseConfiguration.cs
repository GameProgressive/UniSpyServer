using System.Xml.Serialization;
using GameSpyLib.Database;

namespace RetroSpyServer.XMLConfig
{
    public class DatabaseConfiguration
    {
        [XmlAttribute]
        public DatabaseEngine Type;

        public int Port;

        public string Hostname;

        public string Username;

        public string Password;

        public string Databasename;
    }
}
