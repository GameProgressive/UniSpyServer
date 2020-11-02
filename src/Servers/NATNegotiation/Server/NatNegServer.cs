using UniSpyLib.Network;
using NATNegotiation.Handler.CommandSwitcher;
using System.Collections.Concurrent;
using System.Net;

namespace NATNegotiation.Server
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
            //new NATNegotiationPool();
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

            NNCommandSwitcher.Switch(session, message);
        }
    }
}
