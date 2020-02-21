using CDKey.Handler.CommandHandler;
using GameSpyLib.Common;
using GameSpyLib.Database.Entity;
using GameSpyLib.Encryption;
using GameSpyLib.Network;
using System.Collections.Generic;
using System.Net;

namespace CDKey
{
    public class CDKeyServer : TemplateUdpServer
    {
        public static DatabaseEngine DB;
        public CDKeyServer(string serverName, DatabaseEngine engine, IPAddress address, int port) : base(serverName, address, port)
        {
            DB = engine;
        }
        /// <summary>
        ///  Called when a connection comes in on the CDKey server
        ///  known messages
        ///  \ka\ = keep alive from the game server every 20s, we don't care about this
        ///  \auth\ ... = authenticate cd key, this is what we care about
        ///  \disc\ ... = disconnect cd key, because there's checks if the cd key is in use, which we don't care about really, but we could if we wanted to
        /// </summary>
        protected override void OnReceived(EndPoint endPoint, byte[] message)
        {
            string decrypted = Enctypex.XorEncoding(message, 0);
            decrypted.Replace(@"\r\n", "").Replace("\0", "");
            string[] recieved = decrypted.TrimStart('\\').Split('\\');
            Dictionary<string, string> recv = GameSpyUtils.ConvertRequestToKeyValue(recieved);
            CommandSwitcher.Switch(this, endPoint, recv);
        }

        public void UnknownDataRecived(Dictionary<string, string> recv)
        {
            string errorMsg = string.Format("Received unknown data.");
            ToLog(errorMsg);
            GameSpyUtils.PrintReceivedGPDictToLogger(recv);
        }
        private bool _disposed;
        protected override void Dispose(bool disposingManagedResources)
        {
            if (_disposed) return;
            _disposed = true;
            if (disposingManagedResources)
            {

            }
            base.Dispose(disposingManagedResources);
        }

        public override long Send(byte[] buffer, long offset, long size)
        {
            return base.Send(buffer, offset, size);
        }

        public override bool SendAsync(EndPoint endpoint, byte[] buffer)
        {
            return base.SendAsync(endpoint, buffer);
        }
    }
}
