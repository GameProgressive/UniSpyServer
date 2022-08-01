using System.Linq;
using UniSpyServer.Servers.GameTrafficRelay.Handler;
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

            if (((byte[])buffer).Take(6).SequenceEqual(NatNegotiation.Abstraction.BaseClass.RequestBase.MagicData))
            {
                if (Info.PingData is not null && Info.TrafficRelayTarget is not null)
                {
                    // if the received ping packet is already saved in user info, we just send to target client
                    if (Info.PingData.SequenceEqual((byte[])buffer))
                    {
                        LogWriter.LogNetworkTransit(this.Session.RemoteIPEndPoint, Info.TrafficRelayTarget.Session.RemoteIPEndPoint, (byte[])buffer);
                        Info.TrafficRelayTarget.Session.Send((byte[])buffer);
                        return;
                    }
                }

                // if the received ping packet is not saved, we process the ping packet
                base.OnReceived(buffer);
                return;
            }
            // if is not ping packet

            // only binded clients can be sent data
            if (Info.TrafficRelayTarget is not null)
            {
                LogWriter.LogNetworkTransit(this.Session.RemoteIPEndPoint, Info.TrafficRelayTarget.Session.RemoteIPEndPoint, (byte[])buffer);
                Info.TrafficRelayTarget.Session.Send((byte[])buffer);
                return;
            }

        }

        protected override ISwitcher CreateSwitcher(object buffer) => new CmdSwitcher(this, buffer);
    }
}