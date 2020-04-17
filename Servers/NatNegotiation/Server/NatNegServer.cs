using GameSpyLib.Network;
using NatNegotiation.Handler.CommandHandler.CommandSwitcher;
using NatNegotiation.Entity.Structure;
using System.Collections.Concurrent;
using System.Net;

namespace NatNegotiation.Server
{
    public class NatNegServer : TemplateUdpServer
    {
        public static ConcurrentDictionary<EndPoint, NatNegClientInfo> ClientInfoList;
        
        protected readonly ConcurrentDictionary<EndPoint, NatNegClient> Clients;

        public NatNegServer(IPAddress address, int port) : base(address, port)
        {
            ClientInfoList = new ConcurrentDictionary<EndPoint, NatNegClientInfo>();
            Clients = new ConcurrentDictionary<EndPoint, NatNegClient>();
        }

        public override bool Start()
        {
            //new ClientListManager().Start();
            return base.Start();
        }


        protected override object CreateClient(EndPoint endPoint)
        {
            return new NatNegClient(this, endPoint);
        }

        protected override void OnReceived(EndPoint endPoint, byte[] message)
        {
            NatNegClient client;
            if (!Clients.TryGetValue(endPoint, out client))
            {
                client = (NatNegClient)CreateClient(endPoint);
                Clients.TryAdd(endPoint, client);
            }

            new NatNegCommandSwitcher().Switch(client, message);
        }
    }
}
