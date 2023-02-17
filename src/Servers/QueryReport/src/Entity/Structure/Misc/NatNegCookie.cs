using System.Net;
using Newtonsoft.Json;
using UniSpy.Server.Core.MiscMethod;

namespace UniSpy.Server.QueryReport.V2.Entity.Structure.NatNeg
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
