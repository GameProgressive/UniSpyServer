using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Serilog.Events;
using StackExchange.Redis;
using UniSpy.Server.Core.Database;

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
        public string ConnectionString =>
                    $"Server={Server};"
                    + $"Port={Port};"
                    + $"Database={Database};"
                    + $"Username={Username};"
                    + $"Password={Password};"
                    + $"SSL Mode={SSLMode};"
                    + $"Trust Server Certificate={TrustServerCert};"
                    + $"SSL Certificate={SSLCert};"
                    + $"SSL Key={SSLKey};"
                    + $"SSL Password={SSLPassword};"
                    + $"Root Certificate={RootCert};";
        public DatabaseType Type;
        public string Server;
        public int Port;
        public string Database;
        public string Username;
        public string Password;
        public string SSLMode;
        public bool TrustServerCert;
        public string SSLCert;
        public string SSLKey;
        public string SSLPassword;
        public string RootCert;
    }
    public class UniSpyRedisConfig
    {
        public string ConnectionString => $"{Server}:{Port},user={User},password={Password},ssl={SSL},sslHost={SSLHost},abortConnect=false";
        public string Server;
        public int Port;
        public string User;
        public string Password;
        public bool SSL;
        public string SSLHost;
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