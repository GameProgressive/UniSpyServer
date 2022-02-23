using System;
using System.Linq;
using System.Net;

namespace UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass
{
    public abstract class InitResultBase : ResultBase
    {
        public IPEndPoint RemoteIPEndPoint { get; set; }
        public byte[] RemoteIPAddressBytes => RemoteIPEndPoint.Address.GetAddressBytes().ToArray();
        public byte[] RemotePortBytes => BitConverter.GetBytes((ushort)RemoteIPEndPoint.Port).Reverse().ToArray();
    }
}