using System;
using UniSpyServer.Servers.GameTrafficRelay.Controller;
using UniSpyServer.UniSpyLib.Config;
using UniSpyServer.UniSpyLib.Extensions;

namespace UniSpyServer.Servers.GameTrafficRelay.Entity
{
    internal class ServerStatusReporter
    {
        private EasyTimer _myTimer;
        private RedisClient _redisClient;
        private UniSpyServerConfig _config;
        public ServerStatusReporter(UniSpyServerConfig config)
        {
            _redisClient = new RedisClient();
            _config = config;

            UpdateServerInfo();
        }
        public void Start()
        {
            _myTimer = new EasyTimer(TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(10), UpdateServerInfo);
        }
        private void UpdateServerInfo()
        {
            var info = new RelayServerInfo()
            {
                ServerID = _config.ServerID,
                PublicIPEndPoint = _config.PublicIPEndPoint,
                ClientCount = NatNegotiationController.ConnectionPairs.Values.Count * 2
            };
            _redisClient.SetValue(info);
        }
    }
}