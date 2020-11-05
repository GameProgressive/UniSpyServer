using UniSpyLib.Network;
using NATNegotiation.Handler.CommandSwitcher;
using System.Collections.Concurrent;
using System.Net;

namespace NATNegotiation.Network
{
    public class NNServer : UDPServerBase
    {
        public static ConcurrentDictionary<EndPoint, NNSession> Sessions;

        public NNServer(IPAddress address, int port) : base(address, port)
        {
            Sessions = new ConcurrentDictionary<EndPoint, NNSession>();
        }

        public override bool Start()
        {
            //new NATNegotiationPool();
            return base.Start();
        }


        protected override object CreateSession(EndPoint endPoint)
        {
            return new NNSession(this, endPoint);
        }

        protected override void OnReceived(EndPoint endPoint, byte[] message)
        {
            NNSession session;
            if (!Sessions.TryGetValue(endPoint, out session))
            {
                session = (NNSession)CreateSession(endPoint);
                Sessions.TryAdd(endPoint, session);
            }

            NNCommandSwitcher.Switch(session, message);
        }
    }
}
