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

        /// <summary>
        ///  Called when a connection comes in on the CDKey server
        ///  known messages
        ///  \ka\ = keep alive from the game server every 20s, we don't care about this
        ///  \auth\ ... = authenticate cd key, this is what we care about
        ///  \disc\ ... = disconnect cd key, because there's checks if the cd key is in use, which we don't care about really, but we could if we wanted to
        /// </summary>
        protected override void OnReceived(EndPoint endPoint, string message)
        {
            message.Replace(@"\r\n", "").Replace("\0", "");
            string[] keyValueArray = message.TrimStart('\\').Split('\\');
            Dictionary<string, string> recv = GameSpyUtils.ConvertRequestToKeyValue(keyValueArray);
            CommandSwitcher.Switch(this, endPoint, recv);
        }

        protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            byte[] temp = new byte[(int)size];
            Array.Copy(buffer, 0, temp, 0, (int)size);
            string decrypted = XorEncoding.Encrypt(temp, XorEncoding.XorType.Type0);
            LogWriter.ToLog(LogEventLevel.Debug, $"[Recv] {decrypted}");
            OnReceived(endpoint, decrypted);
        }
    }
}
