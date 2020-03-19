using GameSpyLib.Database.Entity;
using GameSpyLib.Network;
using NatNegotiation.Handler.CommandHandler.CommandSwitcher;
using NATNegotiation.Entity.Structure;
using NATNegotiation.Handler.SystemHandler.ClientChecker;
using System;
using System.Collections.Concurrent;
using System.Net;

namespace NatNegotiation
{
    public class NatNegServer : TemplateUdpServer
    {
        public static ConcurrentDictionary<EndPoint, ClientInfo> ClientList = new ConcurrentDictionary<EndPoint, ClientInfo>();

        private ClientListChecker _checker = new ClientListChecker();

        public NatNegServer(string serverName, DatabaseEngine engine, IPAddress address, int port) : base(serverName, address, port)
        {
            _checker.StartCheck(this);
        }

        protected override void OnReceived(EndPoint endPoint, byte[] message)
        {
            if (message.Length < 5)
            {
                return;
            }

            //check and add client into clientList
            ClientInfo client = ClientList.GetOrAdd(endPoint, new ClientInfo(endPoint));
            client.LastPacketTime = DateTime.Now;
            CommandSwitcher.Switch(this, client, message);
        }

        private void CheckClientTimeOut(object sender, System.Timers.ElapsedEventArgs e)
        {
            ToLog("Check timeout excuted!");

            foreach (var c in ClientList.Values)
            {
                //Console.WriteLine(DateTime.Now.Subtract(c.LastPacketTime).TotalSeconds);
                if (DateTime.Now.Subtract(c.LastPacketTime).TotalSeconds > 60)
                {
                    ToLog("Deleted client " + c.RemoteEndPoint.ToString());
                    ClientList.TryRemove(c.RemoteEndPoint, out _);
                }
            }
        }
    }
}
