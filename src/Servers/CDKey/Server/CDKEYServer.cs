using CDKey.Handler.CommandSwitcher;
using UniSpyLib.Encryption;
using UniSpyLib.Logging;
using UniSpyLib.Network;
using Serilog.Events;
using System;
using System.Collections.Concurrent;
using System.Net;

namespace CDKey.Server
{
    public class CDKeyServer : TemplateUdpServer
    {
        protected readonly ConcurrentDictionary<EndPoint, CDKeySession> Sessions
     = new ConcurrentDictionary<EndPoint, CDKeySession>();


        public CDKeyServer(IPAddress address, int port) : base(address, port)
        {
        }

        protected override void OnReceived(EndPoint endPoint, string message)
        {
            CDKeySession session;
            if (!Sessions.TryGetValue(endPoint, out session))
            {
                session = (CDKeySession)CreateSession(endPoint);
                Sessions.TryAdd(endPoint, session);
            }
            CDKeyCommandSwitcher.Switch(session, message);
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

        protected string Decrypt(byte[] buffer, long offset, long size)
        {
            byte[] cipherText = new byte[(int)size];
            Array.Copy(buffer, offset, cipherText, 0, (int)size);
            return XorEncoding.Encrypt(cipherText, XorEncoding.XorType.Type0);
        }

        protected string Encrypt(string plainText)
        {
            return XorEncoding.Encrypt(plainText, XorEncoding.XorType.Type0);
        }

        protected override object CreateSession(EndPoint endPoint)
        {
            return new CDKeySession(this, endPoint);
        }
    }
}
