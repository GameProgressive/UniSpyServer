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
            if(Info.TrafficRelayTarget is not null && (Info.TrafficRelayTarget.Info.TrafficRelayTarget is not null))
            {
                LogWriter.LogNetworkTransit(this.Session.RemoteIPEndPoint, Info.TrafficRelayTarget.Session.RemoteIPEndPoint, (byte[])buffer);
                Info.TrafficRelayTarget.Session.Send((byte[])buffer);
            }
            else
            {
                base.OnReceived(buffer);
            }
            // if (Info.IsTransitNetowrkTraffic)
            // {
            //     LogWriter.LogNetworkTransit(this.Session.RemoteIPEndPoint, Info.TrafficRelayTarget.Session.RemoteIPEndPoint, (byte[])buffer);
            //     Info.TrafficRelayTarget.Session.Send((byte[])buffer);
            // }
            // else
            // {
            //     base.OnReceived(buffer);
            // }
        }
    }
}