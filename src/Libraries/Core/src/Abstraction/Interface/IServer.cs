using System;
using System.Net;
using StackExchange.Redis;

namespace UniSpy.Server.Core.Abstraction.Interface
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
