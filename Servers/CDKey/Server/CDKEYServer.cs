using CDKey.Handler.CommandSwitcher;
using GameSpyLib.Encryption;
using GameSpyLib.Logging;
using GameSpyLib.Network;
using Serilog.Events;
using System;
using System.Collections.Concurrent;
using System.Net;

namespace CDKey.Server
{
    public class CDKeyServer : TemplateUdpServer
    {
        protected readonly ConcurrentDictionary<EndPoint, CDKeyClient> Clients
     = new ConcurrentDictionary<EndPoint, CDKeyClient>();
        public CDKeyServer(IPAddress address, int port) : base(address, port)
        {
        }

        protected override void OnReceived(EndPoint endPoint, string message)
        {
            CDKeyClient client;
            if (!Clients.TryGetValue(endPoint, out client))
            {
                client = (CDKeyClient)CreateClient(endPoint);
                Clients.TryAdd(endPoint, client);
            }
            new CDKeyCommandSwitcher().Switch(client, message);
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

        protected override object CreateClient(EndPoint endPoint)
        {
            return new CDKeyClient(this, endPoint);
        }
    }
}
