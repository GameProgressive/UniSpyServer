using GameSpyLib.Logging;
using System.Xml.Serialization;

namespace GameSpyLib.XMLConfig
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
