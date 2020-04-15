using GameSpyLib.Network;
using NatNegotiation.Handler.CommandHandler.CommandSwitcher;
using NatNegotiation.Entity.Structure;
using System.Collections.Concurrent;
using System.Net;

namespace NatNegotiation.Server
{
    public class NatNegServer : TemplateUdpServer
    {
        public static ConcurrentDictionary<EndPoint, NatNegClientInfo> ClientList = new ConcurrentDictionary<EndPoint, NatNegClientInfo>();

        public NatNegServer(IPAddress address, int port) : base(address, port)
        {
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
            CommandSwitcher.Switch((NatNegClient)CreateClient(endPoint), message);
        }
    }
}
