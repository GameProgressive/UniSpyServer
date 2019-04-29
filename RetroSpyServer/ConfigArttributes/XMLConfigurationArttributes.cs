using System.Xml.Serialization;
using GameSpyLib.Logging;

namespace RetroSpyServer.Config
{
    [XmlRoot("RetroSpy", IsNullable = false)]
    public class XMLConfigurationArttributes
    {
        public GPSPConfigurationArttributes GPSP;        

        public DatabaseConfigurationArttributes Database;

        public ServerConfigurationArttributes[] Servers;

        public LogLevel LogLevel;

        public bool DebugSocket;

        public string BindIP;
    }
}
