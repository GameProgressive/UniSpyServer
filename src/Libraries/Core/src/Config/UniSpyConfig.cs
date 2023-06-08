using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Serilog.Events;
using StackExchange.Redis;

namespace UniSpy.Server.Core.Config
{
    public class UniSpyConfig
    {
        public UniSpyDatabaseConfig Database;
        public UniSpyRedisConfig Redis;
        public List<UniSpyServerConfig> Servers;
        public LogEventLevel MinimumLogLevel;
    }
    public class UniSpyDatabaseConfig
    {
        public string ConnectionString;
    }
    public class UniSpyRedisConfig
    {
        public string ConnectionString;
        [JsonIgnore]
        public IConnectionMultiplexer RedisConnection => ConnectionMultiplexer.Connect(ConnectionString);
    }
    public class UniSpyServerConfig
    {
        public Guid ServerID;
        public string ServerName;
        public IPEndPoint ListeningIPEndPoint => new IPEndPoint(IPAddress.Any, ListeningPort);
        public IPEndPoint PublicIPEndPoint => new IPEndPoint(IPAddress.Parse(PublicAddress), ListeningPort);
        public string PublicAddress;
        public int ListeningPort;
    }
}
