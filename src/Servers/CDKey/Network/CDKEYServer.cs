using System;
using System.Net;
using CDKey.Handler.CmdSwitcher;
using UniSpyLib.Encryption;
using UniSpyLib.Network;

namespace CDKey.Network
{
    internal sealed class CDKeyServer : UniSpyUDPServerBase
    {
        public CDKeyServer(Guid serverID, IPEndPoint endpoint) : base(serverID, endpoint)
        {
            SessionManager = new CDKeySessionManager();
        }
        protected override UniSpyUDPSessionBase CreateSession(EndPoint endPoint) => new CDKeySession(this, endPoint);

        protected override void OnReceived(UniSpyUDPSessionBase session, string message)
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
