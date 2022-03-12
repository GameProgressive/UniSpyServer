using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.NatNegotiation.Handler.CmdHandler
{
    public sealed class PingHandler : CmdHandlerBase
    {
        /// <summary>
        /// This is the client pool for storage the clients which traffic need to be redirected
        /// |key: "cookie, clientIndex"| value: Client|
        /// </summary>
        /// <typeparam name="Client"></typeparam>
        /// <returns></returns>
        public static IDictionary<string, Client> TrafficRedirectClientPool = new ConcurrentDictionary<string, Client>();
        private new PingRequest _request => (PingRequest)base._request;
        private Client _targetClient;
        public PingHandler(IClient client, IRequest request) : base(client, request)
        {
        }

        protected override void RequestCheck()
        {
            // if our client is not in the pool, we will add it
            if (!TrafficRedirectClientPool.ContainsKey($"{_request.Cookie},{_request.ClientIndex}"))
            {
                TrafficRedirectClientPool.TryAdd($"{_request.Cookie},{_request.ClientIndex}", _client);
            }
            // we check the other client
            var otherIndex = (NatClientIndex)((int)NatClientIndex.GameServer - _request.ClientIndex);
            var waitExpireTime = TimeSpan.FromSeconds(5);
            var startTime = DateTime.Now;
            while (DateTime.Now.Subtract(startTime) < waitExpireTime)
            {
                if (TrafficRedirectClientPool.ContainsKey($"{_request.Cookie},{otherIndex}"))
                {
                    break;
                }
                Thread.Sleep(1000);
            }
            if (!TrafficRedirectClientPool.ContainsKey($"{_request.Cookie},{otherIndex}"))
            {
                return;
            }
            _targetClient = TrafficRedirectClientPool[$"{_request.Cookie},{otherIndex}"];
        }

        protected override void DataOperation()
        {
            lock (_client.Info)
            {
                if (_client.Info.IsTransitNetowrkTraffic != true)
                {
                    _client.Info.IsTransitNetowrkTraffic = true;
                }
                if (_client.Info.ClientIndex is null)
                {
                    _client.Info.ClientIndex = _request.ClientIndex;
                }
                if (_client.Info.TrafficTransitTarget is null)
                {
                    _client.Info.TrafficTransitTarget = _targetClient;
                }
            }
        }
    }
}