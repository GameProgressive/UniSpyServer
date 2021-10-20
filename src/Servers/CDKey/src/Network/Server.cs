using UniSpyServer.CDkey.Handler.CmdSwitcher;
using System;
using System.Net;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Udp.Server;
using UniSpyServer.UniSpyLib.Encryption;

namespace UniSpyServer.CDkey.Network
{
    public sealed class Server : UniSpyUdpServer
    {
        public Server(Guid serverID, IPEndPoint endpoint) : base(serverID, endpoint)
        {
            SessionManager = new SessionManager();
        }
        protected override UniSpyUdpSession CreateSession(EndPoint endPoint) => new Session(this, endPoint);

        protected override byte[] Decrypt(byte[] buffer)
        {
            return XOREncoding.Encode(buffer, XOREncoding.XorType.Type0);
        }
        protected override byte[] Encrypt(byte[] buffer)
        {
            return XOREncoding.Encode(buffer, XOREncoding.XorType.Type0);
        }
    }
}
