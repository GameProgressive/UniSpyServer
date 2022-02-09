using System.Net;

namespace UniSpyServer.Servers.QueryReport.Entity.Structure.NATNeg
{
    public record NatNegCookie
    {
        public IPAddress HostIPAddress { get; init; }
        public ushort HostPort { get; init; }
        public IPEndPoint HeartBeatIPEndPoint { get; init; }
        public string GameName { get; init; }
        public byte[] NatNegMessage { get; init; }
        public uint InstantKey { get; init; }

        public NatNegCookie()
        {
        }
    }
}
