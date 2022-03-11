using System;
using System.Linq;
using System.Net;

namespace UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass
{
    public abstract class CommonResultBase : ResultBase
    {
        public IPEndPoint PublicIPEndPoint { get; set; }
        public byte[] RemoteIPAddressBytes => PublicIPEndPoint.Address.GetAddressBytes().ToArray();
        public byte[] RemotePortBytes => BitConverter.GetBytes((ushort)PublicIPEndPoint.Port).Reverse().ToArray();
    }
}