using System.Xml.Serialization;

namespace RetroSpyServer.Config
{
    public class ServerConfiguration
    {
        public string Hostname;

        public int MaxConnections;

        public bool Disabled;

        public int Port;
    }
}
