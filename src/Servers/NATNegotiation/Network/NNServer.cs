using UniSpyLib.Network;
using NATNegotiation.Handler.CommandSwitcher;
using System.Net;

namespace NATNegotiation.Network
{
    public class NNServer : UDPServerBase
    {


        public NNServer(IPAddress address, int port) : base(address, port)
        {
        }

        protected override UDPSessionBase CreateSession(EndPoint endPoint)
        {
            return new NNSession(this, endPoint);
        }

        //protected override void OnReceived(EndPoint endPoint, byte[] message)
        //{
        //    NNSession session;
        //    if (!Sessions.TryGetValue(endPoint, out session))
        //    {
        //        session = (NNSession)CreateSession(endPoint);
        //        Sessions.TryAdd(endPoint, session);
        //    }

        //    NNCommandSwitcher.Switch(session, message);
        //}

        protected override void OnReceived(UDPSessionBase session, byte[] message)
        {
            base.OnReceived(session, message);
            new NNCommandSerializer(session, message).Serialize();
        }
    }
}
