using System;
using System.Linq;
using System.Net;
using System.Timers;
using Newtonsoft.Json;
using StackExchange.Redis;
using UniSpyServer.LinqToRedis;
using UniSpyServer.Servers.GameTrafficRelay.Entity.Structure;
using UniSpyServer.UniSpyLib.Config;
using UniSpyServer.UniSpyLib.Extensions;
using UniSpyServer.UniSpyLib.MiscMethod;

namespace UniSpyServer.Servers.GameTrafficRelay.Entity.Structure.Redis
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
            var clientCount = Client.ClientPool.Values.Where(x => ((Client)x).Info.TrafficRelayTarget is not null).Count();
            _info = new RelayServerInfo()
            {
                ServerID = config.ServerID,
                PublicIPEndPoint = config.ListeningEndPoint,
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