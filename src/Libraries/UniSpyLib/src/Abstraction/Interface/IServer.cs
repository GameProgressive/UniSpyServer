using System;
using System.Collections.Concurrent;
using System.Net;
using StackExchange.Redis;
using UniSpyServer.UniSpyLib.Config;

namespace UniSpyServer.UniSpyLib.Abstraction.Interface
{
    public interface IServer
    {
        static IConnectionMultiplexer RedisConnection { get; }
        Guid ServerID { get; }
        string ServerName { get; }
        IPEndPoint ListeningIPEndPoint { get; }
        IPEndPoint PublicIPEndPoint { get; }
        // UniSpyServerConfig ServerConfig { get; }
        void Start();
    }
}
