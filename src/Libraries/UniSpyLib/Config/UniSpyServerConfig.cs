using System;
using System.Net;
using Newtonsoft.Json;

namespace UniSpyLib.UniSpyConfig
{
    public class UniSpyServerConfig
    {
        public Guid ServerID;
        public string ServerName;
        public IPEndPoint ListeningEndPoint => new IPEndPoint(IPAddress.Parse(ListeningAddress), ListeningPort);
        public string ListeningAddress;
        public int ListeningPort;
        public string RemoteAddress;
        public int RemotePort;
    }
}
