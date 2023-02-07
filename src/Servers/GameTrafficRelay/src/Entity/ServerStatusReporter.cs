using System.Net;
using System;
using System.Timers;
using UniSpyServer.Servers.GameTrafficRelay.Controller;
using UniSpyServer.UniSpyLib.Config;
using UniSpyServer.UniSpyLib.Extensions;

namespace UniSpyServer.Servers.GameTrafficRelay.Entity
{
    internal class ServerStatusReporter
    {
        private EasyTimer _myTimer;
        private RelayServerInfo _info;
        private RedisClient _redisClient;
        public ServerStatusReporter(UniSpyServerConfig config)
        {
            _redisClient = new RedisClient();
            var clientCount = NatNegotiationController.ConnectionPairs.Values.Count * 2;
            _info = new RelayServerInfo()
            {
                ServerID = config.ServerID,
                PublicIPEndPoint = config.PublicIPEndPoint,
                ClientCount = clientCount
            };
            UpdateServerInfo();
        }
        public void Start()
        {
            _myTimer = new EasyTimer(TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(10), UpdateServerInfo);
        }
        private void UpdateServerInfo()
        {
            _info.ClientCount = NatNegotiationController.ConnectionPairs.Count * 2;
            _redisClient.SetValue(_info);
        }

    }
}