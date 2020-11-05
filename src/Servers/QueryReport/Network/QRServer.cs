using UniSpyLib.Network;
using QueryReport.Handler.CommandSwitcher;
using QueryReport.Handler.SystemHandler.NatNegCookieManage;
using QueryReport.Handler.SystemHandler.PeerSystem;
using QueryReport.Handler.SystemHandler.QRSessionManage;
using QueryReport.Handler.SystemHandler.ServerList;
using System.Net;

namespace QueryReport.Network
{
    public class QRServer : UDPServerBase
    {



        public bool IsChallengeSent;

        public bool HasInstantKey;

        public QRServer(IPAddress address, int port) : base(address, port)
        {
            IsChallengeSent = false;
            HasInstantKey = false;
        }

        public override bool Start()
        {
            new PeerGroupHandler().LoadAllGameGroupsToRedis();
            //The Time for servers to remain in the serverlist since the last ping in seconds.
            //This value must be greater than 20 seconds, as that is the ping rate of the server
            //Suggested value is 30 seconds, this gives the server some time if the master server
            //is busy and cant refresh the server's TTL right away
            new QRSessionManager().Start();
            new NatNegCookieManager().Start();
            new ServerListManager().Start();
            return base.Start();
        }

        protected override object CreateSession(EndPoint endPoint)
        {
            return new QRSession(this, endPoint);
        }

        protected override void OnReceived(EndPoint endPoint, byte[] message)
        {
            QRSession session;
            if (!QRSessionManager.Sessions.TryGetValue(endPoint, out session))
            {
                session = (QRSession)CreateSession(endPoint);
                QRSessionManager.Sessions.TryAdd(endPoint, session);
            }
            QRCommandSwitcher.Switch(session, message);
        }
    }
}
