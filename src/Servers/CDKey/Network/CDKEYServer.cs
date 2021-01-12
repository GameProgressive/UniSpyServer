using CDKey.Handler.CmdSwitcher;
using UniSpyLib.Encryption;
using UniSpyLib.Logging;
using UniSpyLib.Network;
using Serilog.Events;
using System;
using System.Collections.Concurrent;
using System.Net;

namespace CDKey.Network
{
    internal sealed class CDKeyServer : UDPServerBase
    {
        public CDKeyServer(IPAddress address, int port) : base(address, port)
        {
        }

        protected override void OnReceived(UDPSessionBase session, string message)
        {
           new CDKeyCmdSwitcher(session, message).Switch();
        }

        protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            string decrypted = Decrypt(buffer, offset, size);
            LogWriter.ToLog(LogEventLevel.Debug, $"[Recv] {decrypted}");
            OnReceived(endpoint, decrypted);
        }

        public override bool SendAsync(EndPoint endpoint, string text)
        {
            return BaseSendAsync(endpoint, Encrypt(text));
        }

        private string Decrypt(byte[] buffer, long offset, long size)
        {
            byte[] cipherText = new byte[(int)size];
            Array.Copy(buffer, offset, cipherText, 0, (int)size);
            return XorEncoding.Encrypt(cipherText, XorEncoding.XorType.Type0);
        }

        private string Encrypt(string plainText)
        {
            return XorEncoding.Encrypt(plainText, XorEncoding.XorType.Type0);
        }

        protected override UDPSessionBase CreateSession(EndPoint endPoint)
        {
            return new CDKeySession(this, endPoint);
        }
    }
}
