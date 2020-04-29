using GameSpyLib.Network;
using NatNegotiation.Entity.Structure;
using NatNegotiation.Handler.CommandHandler.CommandSwitcher;
using System.Collections.Concurrent;
using System.Net;

namespace NatNegotiation.Server
{
    public class NatNegServer : TemplateUdpServer
    {
        public static ConcurrentDictionary<EndPoint, NatNegSession> Sessions;

        public NatNegServer(IPAddress address, int port) : base(address, port)
        {
            Sessions = new ConcurrentDictionary<EndPoint, NatNegSession>();
        }

        public override bool Start()
        {
            //new ClientListManager().Start();
            return base.Start();
        }


        protected override object CreateSession(EndPoint endPoint)
        {
            return new NatNegSession(this, endPoint);
        }

        protected override void OnReceived(EndPoint endPoint, byte[] message)
        {
            NatNegSession session;
            if (!Sessions.TryGetValue(endPoint, out session))
            {
                session = (NatNegSession)CreateSession(endPoint);
                Sessions.TryAdd(endPoint, session);
            }

            new NatNegCommandSwitcher().Switch(session, message);
        }
    }
}
