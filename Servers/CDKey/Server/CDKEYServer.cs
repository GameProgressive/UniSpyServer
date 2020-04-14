using CDKey.Handler.CommandSwitcher;
using GameSpyLib.Encryption;
using GameSpyLib.Logging;
using GameSpyLib.MiscMethod;
using GameSpyLib.Network;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Net;

namespace CDKey
{
    public class CDKeyServer : TemplateUdpServer
    {
        public CDKeyServer(IPAddress address, int port) : base(address, port)
        {
        }

        protected override void OnReceived(EndPoint endPoint, string message)
        {
            message.Replace(@"\r\n", "").Replace("\0", "");
            string[] keyValueArray = message.TrimStart('\\').Split('\\');
            Dictionary<string, string> recv = GameSpyUtils.ConvertRequestToKeyValue(keyValueArray);
            CommandSwitcher.Switch(this, endPoint, recv);
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
    }
}
