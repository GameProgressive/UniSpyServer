using System.Net;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Enumerate;

namespace NATNegotiation.Entity.Structure.Result
{
    public class ConnectResult : NNResultBase
    {
        public byte? GotYourData { get; set; }
        public byte? Finished { get; set; }
        public EndPoint RemoteEndPoint { get; set; }
        public byte Version;
        public uint Cookie;
        public ConnectResult()
        {
            PacketType = NatPacketType.Connect;
        }
    }
}
