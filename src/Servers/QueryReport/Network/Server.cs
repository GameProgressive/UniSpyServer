﻿using QueryReport.Entity.Structure.Redis;
using QueryReport.Handler.CmdSwitcher;
using QueryReport.Handler.SystemHandler;
using System;
using System.Net;
using UniSpyLib.Abstraction.BaseClass.Network.Udp.Server;

namespace QueryReport.Network
{
    internal sealed class Server : UniSpyUdpServer
    {
        public RedisChannelSubscriber RedisChannelSubscriber { get; private set; }
        public Server(Guid serverID, IPEndPoint endpoint) : base(serverID, endpoint)
        {
            SessionManager = new SessionManager();
            RedisChannelSubscriber = new RedisChannelSubscriber();
        }

        public override bool Start()
        {
            PeerGroupInfoRedisOperator.LoadAllGameGroupsToRedis();
            RedisChannelSubscriber.StartSubscribe();
            return base.Start();
        }

        protected override UniSpyUdpSession CreateSession(EndPoint endPoint) => new Session(this, endPoint);


    }
}
