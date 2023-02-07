using System.Net;
using System;
using System.Timers;
using UniSpyServer.Servers.GameTrafficRelay.Controller;
using UniSpyServer.UniSpyLib.Config;

namespace UniSpyServer.Servers.GameTrafficRelay.Entity
{
    class ServerStatusReporter
    {
        private Timer _timer;
        private TimeSpan _expireTimeInterval;
        private RelayServerInfo _info;
        private RedisClient _redisClient;
        public ServerStatusReporter(UniSpyServerConfig config)
        {
            _timer = new Timer(60000);
            _expireTimeInterval = new TimeSpan(0, 1, 0);
            _redisClient = new RedisClient();
            var clientCount = NatNegotiationController.ConnectionPairs.Values.Count * 2;
            _info = new RelayServerInfo()
            {
                ServerID = config.ServerID,
                PublicIPEndPoint = config.PublicIPEndPoint,
                ClientCount = clientCount
            };
        }
        public void Start()
        {
            UpdateServerInfo();
            _timer.Elapsed += (s, e) => UpdateServerInfo();
            _timer.Start();
        }
        private void UpdateServerInfo()
        {
            _redisClient.SetValue(_info);
        }
    }
}