using CDKey.Handler.CmdSwitcher;
using System;
using System.Net;
using UniSpyLib.Abstraction.BaseClass.Network.Udp.Server;
using UniSpyLib.Encryption;

namespace CDKey.Network
{
    internal sealed class CDKeyServer : UniSpyUdpServer
    {
        public CDKeyServer(Guid serverID, IPEndPoint endpoint) : base(serverID, endpoint)
        {
            SessionManager = new CDKeySessionManager();
        }
        protected override UniSpyUdpSession CreateSession(EndPoint endPoint) => new CDKeySession(this, endPoint);

        protected override void OnReceived(UniSpyUdpSession session, string message)
            => new CDKeyCmdSwitcher(session, message).Switch();

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
