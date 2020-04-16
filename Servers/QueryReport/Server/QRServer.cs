using GameSpyLib.Extensions;
using GameSpyLib.Network;
using QueryReport.Entity.Structure.Group;
using QueryReport.Handler.CommandHandler.ServerList;
using QueryReport.Handler.CommandSwitcher;
using QueryReport.Handler.SystemHandler.PeerSystem;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;

namespace QueryReport.Server
{
    public class QRServer : TemplateUdpServer
    {

        public static ConcurrentDictionary<EndPoint, QRClient> Clients
                                         = new ConcurrentDictionary<EndPoint, QRClient>();
        public bool IsChallengeSent = false;

        public bool HasInstantKey = false;

        public QRServer(IPAddress address, int port) : base(address, port)
        {
        }

        public override bool Start()
        {
            new PeerGroupHandler().LoadAllGameGroupsToRedis();
            //The Time for servers to remain in the serverlist since the last ping in seconds.
            //This value must be greater than 20 seconds, as that is the ping rate of the server
            //Suggested value is 30 seconds, this gives the server some time if the master server
            //is busy and cant refresh the server's TTL right away
            new ServerListManager().Start();
            return base.Start();
        }

        protected override object CreateClient(EndPoint endPoint)
        {
            return new QRClient(this, endPoint);
        }

        protected override void OnReceived(EndPoint endPoint, byte[] message)
        {
            QRClient client;
            if (!Clients.TryGetValue(endPoint, out client))
            {
                client = (QRClient)CreateClient(endPoint);
                Clients.TryAdd(endPoint, client);
            }

            CommandSwitcher.Switch(client, message);
        }
    }
}
