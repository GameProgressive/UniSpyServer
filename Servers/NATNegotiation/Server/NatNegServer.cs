using GameSpyLib.Database.Entity;
using GameSpyLib.Network;
using NatNegotiation.Handler.CommandHandler.CommandSwitcher;
using NATNegotiation.Entity.Structure;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;

namespace NatNegotiation
{
    public class NatNegServer : TemplateUdpServer
    {
        public static ConcurrentDictionary<EndPoint, ClientInfo> ClientList = new ConcurrentDictionary<EndPoint, ClientInfo>();
        private System.Timers.Timer _CheckTimer = new System.Timers.Timer { Enabled = true, Interval = 10000, AutoReset = true };


        public NatNegServer(string serverName, DatabaseEngine engine, IPAddress address, int port) : base(serverName, address, port)
        {
            _CheckTimer.Start();
            _CheckTimer.Elapsed+=CheckClientTimeOut;
        }

        protected override void OnReceived(EndPoint endPoint, byte[] message)
        {
            if (message.Length < 5)
                return;
            //check and add client into clientList

            //ClientInfo client = ClientList.Where(c => c.EndPoint == endPoint).First();
            ClientInfo client;
                client = ClientList.GetOrAdd(endPoint, new ClientInfo { EndPoint = endPoint, ConnectTime = DateTime.Now });
                client.Parse(message);
            CommandSwitcher.Switch(this, client, message);
        }

        private void CheckClientTimeOut(object sender, System.Timers.ElapsedEventArgs e)
        {
            ToLog("Check timeout excuted!");
            foreach (var c in ClientList.Values)
            {
                if ((DateTime.Now - c.LastPacketTime).Seconds > 60)
                {
                    ToLog("Deleted client "+c.EndPoint.ToString());
                    ClientList.TryRemove(c.EndPoint, out _);
                }
            }
        }
    }
}
