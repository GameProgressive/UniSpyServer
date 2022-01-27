using System;
using System.Net;

namespace UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass
{
    public abstract class InitResultBase : ResultBase
    {
        public IPEndPoint RemoteIPEndPoint { get; set; }
        public byte[] RemoteIPBytes => RemoteIPEndPoint.Address.GetAddressBytes();
        public byte[] RemotePortBytes => BitConverter.GetBytes((ushort)RemoteIPEndPoint.Port);
    }
}