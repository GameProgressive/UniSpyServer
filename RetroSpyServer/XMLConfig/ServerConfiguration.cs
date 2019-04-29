using System.Xml.Serialization;

namespace RetroSpyServer.XMLConfig
{
    public class ServerConfiguration
    {
        [XmlAttribute]
        public string Name;

        public string Hostname;

        public int MaxConnections;

        public bool Disabled;

        public int Port;
    }
}
