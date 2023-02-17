using System;
using System.Linq;
using System.Net;
using UniSpy.Server.NatNegotiation.Abstraction.BaseClass;
using UniSpy.Server.NatNegotiation.Enumerate;

namespace UniSpy.Server.NatNegotiation.Contract.Result
{
    public class ConnectResult : ResultBase
    {
        public byte? GotYourData { get; private set; }
        public ConnectPacketStatus? Finished { get; private set; }
        public IPEndPoint RemoteEndPoint { get; set; }
        public byte[] RemoteIPAddressBytes => RemoteEndPoint.Address.GetAddressBytes();
        public byte[] RemotePortBytes => BitConverter.GetBytes((ushort)RemoteEndPoint.Port).Reverse().ToArray();
        public byte Version;
        public int Cookie;
        public ConnectResult()
        {
            PacketType = ResponseType.Connect;
            GotYourData = 1;
            Finished = ConnectPacketStatus.NoError;
        }
    }
}
