using System;
using System.Net;
using System.Timers;
using Newtonsoft.Json;
using StackExchange.Redis;
using UniSpyServer.LinqToRedis;
using UniSpyServer.Servers.GameTrafficRelay.Entity.Structure;
using UniSpyServer.UniSpyLib.Config;
using UniSpyServer.UniSpyLib.Extensions;
using UniSpyServer.UniSpyLib.MiscMethod;

namespace UniSpyServer.Servers.GameTrafficRelay.Entity.Redis
{
    record RelayServerInfo : RedisKeyValueObject
    {
        [RedisKey]
        public Guid ServerID { get; init; }
        [RedisKey]
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint PublicIPEndPoint { get; init; }

        public RelayServerInfo() : base(TimeSpan.FromMinutes(1))
        {
        }
    }

    class RedisClient : UniSpyServer.LinqToRedis.RedisClient<RelayServerInfo>
    {
        public RedisClient() : base(Client.RedisConnection, (int)DbNumber.GameTrafficRelay) { }
    }
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
            _info = new RelayServerInfo()
            {
                ServerID = config.ServerID,
                PublicIPEndPoint = config.ListeningEndPoint
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