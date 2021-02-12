using QueryReport.Handler.CmdSwitcher;
using QueryReport.Handler.SystemHandler.Redis;
using QueryReport.Handler.SystemHandler.ServerList;
using System.Net;
using UniSpyLib.Network;

namespace QueryReport.Network
{
    public class QRServer : UniSpyUDPServerBase
    {
        public QRServer(IPAddress address, int port) : base(address, port)
        {
        }

        public override bool Start()
        {
            new ServerListManager().Start();
            PeerGroupInfoRedisOperator.LoadAllGameGroupsToRedis();
            return base.Start();
        }

        protected override UniSpyUDPSessionBase CreateSession(EndPoint endPoint)
        {
            return new QRSession(this, endPoint);
        }

        protected override void OnReceived(UniSpyUDPSessionBase session, byte[] message)
        {
            base.OnReceived(session, message);
            new QRCmdSwitcher(session, message).Switch();
        }
    }
}
