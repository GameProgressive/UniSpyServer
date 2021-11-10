using System;
using System.Net;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;

namespace UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass
{
    public abstract class InitResultBase : ResultBase
    {
        public IPEndPoint RemoteIPEndPoint { get; set; }
        public byte[] RemoteIPBytes => RemoteIPEndPoint.Address.GetAddressBytes();
        public byte[] RemotePortBytes => BitConverter.GetBytes((ushort)RemoteIPEndPoint.Port);
    }
}