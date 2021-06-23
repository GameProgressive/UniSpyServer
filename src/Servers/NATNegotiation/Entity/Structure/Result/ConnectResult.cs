using NatNegotiation.Abstraction.BaseClass;
using NatNegotiation.Entity.Enumerate;
using System.Net;
using UniSpyLib.Extensions;

namespace NatNegotiation.Entity.Structure.Result
{
    public class ConnectResult : NNResultBase
    {
        public byte? GotYourData { get; set; }
        public byte? Finished { get; set; }
        public IPEndPoint RemoteEndPoint { private get; set; }
        public byte[] RemoteIPAddress =>
            HtonsExtensions.EndPointToIPBytes(RemoteEndPoint);
        public byte[] RemotePort =>
            HtonsExtensions.EndPointToHtonsPortBytes(RemoteEndPoint);

        public byte Version;
        public uint Cookie;
        public ConnectResult()
        {
            PacketType = NatPacketType.Connect;
        }
    }
}
