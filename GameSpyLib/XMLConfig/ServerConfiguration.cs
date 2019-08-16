using System.Xml.Serialization;

namespace GameSpyLib.XMLConfig
{
    public class ServerConfiguration
    {
        [XmlAttribute]
        public string Name;

        public string Hostname;

        public int MaxConnections;

        public int Port;

        //public bool Disabled;
    }
}
