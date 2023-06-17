using System;
using UniSpy.Server.GameTrafficRelay.Controller;
using UniSpy.Server.Core.Extension;

namespace UniSpy.Server.GameTrafficRelay.Entity
{
    internal class ServerStatusReporter
    {
        private EasyTimer _myTimer;
        private RedisClient _redisClient = new RedisClient();
        private Core.Abstraction.Interface.IServer _server;
        public ServerStatusReporter(Core.Abstraction.Interface.IServer server)
        {
            _server = server;
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
                ServerID = _server.Id,
                PublicIPEndPoint = _server.PublicIPEndPoint,
                ClientCount = NatNegotiationController.ConnectionPairs.Values.Count * 2
            };
            _ = _redisClient.SetValueAsync(info);
            var data = _redisClient.GetKeyValues();
        }
    }
}