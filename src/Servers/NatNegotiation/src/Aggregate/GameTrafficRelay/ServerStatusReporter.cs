using System;
using UniSpy.Server.Core.Extension;

namespace UniSpy.Server.NatNegotiation.Aggregate.GameTrafficRelay
{
    public class ServerStatusReporter
    {
        private EasyTimer _myTimer;
        private RelayServerCache.RedisClient _redisClient = new();
        private Core.Abstraction.Interface.IServer _server;
        public ServerStatusReporter(Core.Abstraction.Interface.IServer server)
        {
            _server = server;
            _myTimer = new EasyTimer(TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(10));
            _myTimer.Elapsed += (s, e) => UpdateServerInfo();
            UpdateServerInfo();
        }
        public void Start()
        {
            _myTimer.Start();
        }
        private void UpdateServerInfo()
        {
            var info = new RelayServerCache()
            {
                ServerID = _server.Id,
                PublicIPEndPoint = _server.PublicIPEndPoint,
                ClientCount = Handler.CmdHandler.PingHandler.ConnectionListeners.Values.Count
            };
            _ = _redisClient.SetValueAsync(info);
            var data = _redisClient.GetKeyValues();
        }
    }
}