using System.Net;
using Newtonsoft.Json;
using UniSpyServer.UniSpyLib.MiscMethod;

namespace UniSpyServer.Servers.QueryReport.Entity.Structure.NATNeg
{
    public record NatNegCookie
    {
        [JsonConverter(typeof(IPAddresConverter))]
        public IPAddress HostIPAddress { get; init; }
        public ushort HostPort { get; init; }
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint HeartBeatIPEndPoint { get; init; }
        public string GameName { get; init; }
        public byte[] NatNegMessage { get; init; }
        public uint InstantKey { get; init; }

        public NatNegCookie()
        {
        }
    }
}
