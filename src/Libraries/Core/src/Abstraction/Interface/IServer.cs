using System;
using System.Net;
using StackExchange.Redis;

namespace UniSpy.Server.Core.Abstraction.Interface
{
    /// <summary>
    /// Represents unispy server instance
    /// </summary>
    public interface IServer
    {
        static IConnectionMultiplexer RedisConnection { get; }
        Guid Id { get; }
        string Name { get; }
        IPEndPoint ListeningIPEndPoint { get; }
        IPEndPoint PublicIPEndPoint { get; }
        // UniSpyServerConfig ServerConfig { get; }
        void Start();
    }
}
