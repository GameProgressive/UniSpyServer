using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.GameTrafficRelay.Entity.Structure
{
    public class Client : ClientBase
    {
        public new ClientInfo Info { get => (ClientInfo)base.Info; private set => base.Info = value; }
        public new IUdpSession Session => (IUdpSession)base.Session;
        public Client(ISession session) : base(session)
        {
            Info = new ClientInfo();
        }

        protected override void OnReceived(object buffer)
        {
            // if is ping packet
            if (((byte[])buffer).Take(5).SequenceEqual(NatNegotiation.Abstraction.BaseClass.RequestBase.MagicData))
            {
                // if the received ping packet is already saved in user info, we just send to target client
                if (Info.PingData.SequenceEqual((byte[])buffer))
                {
                    LogWriter.LogNetworkTransit(this.Session.RemoteIPEndPoint, Info.TrafficRelayTarget.Session.RemoteIPEndPoint, (byte[])buffer);
                    Info.TrafficRelayTarget.Session.Send((byte[])buffer);
                }
                else
                {
                    // if the received ping packet is not saved, we process the ping packet
                    base.OnReceived(buffer);
                }
            }
            // if is not ping packet
            else
            {
                // only binded clients can be sent data
                if (Info.TrafficRelayTarget is not null)
                {
                    LogWriter.LogNetworkTransit(this.Session.RemoteIPEndPoint, Info.TrafficRelayTarget.Session.RemoteIPEndPoint, (byte[])buffer);
                    Info.TrafficRelayTarget.Session.Send((byte[])buffer);
                }
            }
        }
    }
}