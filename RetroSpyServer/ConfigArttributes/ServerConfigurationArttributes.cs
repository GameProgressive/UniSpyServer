using System.Xml.Serialization;

namespace RetroSpyServer.Config
{
    public class ServerConfigurationArttributes
    {
        public string Hostname;

        public string Name;

        public int MaxConnections;

        public bool Disabled;

        public int Port;
    }
}
