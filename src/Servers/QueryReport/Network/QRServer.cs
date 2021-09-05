using QueryReport.Entity.Structure.Redis;
using QueryReport.Handler.CmdSwitcher;
using QueryReport.Handler.SystemHandler;
using System;
using System.Net;
using UniSpyLib.Abstraction.BaseClass.Network.Udp.Server;

namespace QueryReport.Network
{
    internal sealed class QRServer : UniSpyUdpServer
    {
        public QRRedisChannelSubscriber RedisChannelSubscriber { get; private set; }
        public QRServer(Guid serverID, IPEndPoint endpoint) : base(serverID, endpoint)
        {
            SessionManager = new QRSessionManager();
            RedisChannelSubscriber = new QRRedisChannelSubscriber();
        }

        public override bool Start()
        {
            PeerGroupInfoRedisOperator.LoadAllGameGroupsToRedis();
            RedisChannelSubscriber.StartSubscribe();
            return base.Start();
        }

        protected override UniSpyUdpSession CreateSession(EndPoint endPoint)
            => new QRSession(this, endPoint);

        protected override void OnReceived(UniSpyUdpSession session, byte[] message)
            => new QRCmdSwitcher(session, message).Switch();

    }
}
