using System.Xml.Serialization;
using GameSpyLib.Logging;

namespace RetroSpyServer.Config
{
    [XmlRoot("RetroSpy", IsNullable = false)]
    public class XMLConfiguration
    {
        public GPSPConfiguration GPSP;

        public DatabaseConfiguration Database;

        public LogLevel LogLevel;

        public bool DebugSocket;

        public string BindIP;
    }
}
