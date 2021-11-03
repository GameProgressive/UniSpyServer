using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using System;
using System.Linq;
using System.Net;
using UniSpyServer.UniSpyLib.Extensions;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Result
{
    public class ConnectResult : ResultBase
    {
        public byte? GotYourData { get; set; }
        public byte? Finished { get; set; }
        public IPEndPoint RemoteEndPoint { private get; set; }
        public byte[] RemoteIPAddressBytes => RemoteEndPoint.Address.GetAddressBytes();
        public byte[] RemotePortBytes => BitConverter.GetBytes((ushort)RemoteEndPoint.Port).Reverse().ToArray();
        public byte Version;
        public uint Cookie;
        public ConnectResult()
        {
            PacketType = NatPacketType.Connect;
        }
    }
}
