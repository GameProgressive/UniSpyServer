using System;
using System.Collections.Generic;
using System.Net;
using Serilog.Events;
using UniSpyServer.UniSpyLib.Database;

namespace UniSpyServer.UniSpyLib.Config
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
        public string ConnectionString => $"{Server}:{Port},user={User},password={Password},ssl={SSL},sslHost={SSLHost}";
        public string Server;
        public int Port;
        public string User;
        public string Password;
        public bool SSL;
        public string SSLHost;
    }
    public class UniSpyServerConfig
    {
        public Guid ServerID;
        public string ServerName;
        public IPEndPoint ListeningEndPoint => new IPEndPoint(IPAddress.Parse(ListeningAddress), ListeningPort);
        public string ListeningAddress;
        public int ListeningPort;
        public string RemoteAddress;
        public int RemotePort;
    }
}
