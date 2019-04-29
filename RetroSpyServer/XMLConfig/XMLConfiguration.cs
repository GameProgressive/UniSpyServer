using System.Xml.Serialization;
using GameSpyLib.Logging;

namespace RetroSpyServer.XMLConfig
{
    [XmlRoot("RetroSpy", IsNullable = false)]
    public class XMLConfiguration
    {
        [XmlArray("Servers")]
        public ServerConfiguration[] Servers;      

        [XmlElement("Database", IsNullable = false)]
        public DatabaseConfiguration Database;

        public LogLevel LogLevel;
    }
}
