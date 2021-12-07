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
                    $"Server={RemoteAddress};"
                    + $"Database={DatabaseName};"
                    + $"Uid={UserName};"
                    + $"Pwd={Password};"
                    + $"Port={RemotePort};"
                    + $"SslMode={SslMode};"
                    + $"SslCert={SslCert};"
                    + $"SslKey={SslKey};"
                    + $"SslCa={SslCa}";
        public DatabaseType Type;
        public string RemoteAddress;
        public int RemotePort;
        public string UserName;
        public string Password;
        public string DatabaseName;
        public string SslMode;
        public string SslCert;
        public string SslKey;
        public string SslCa;
    }
    public class UniSpyRedisConfig
    {
        public string ConnectionString => $"{RemoteAddress}:{RemotePort}";
        public string RemoteAddress;
        public int RemotePort;
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
