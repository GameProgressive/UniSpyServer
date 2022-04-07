using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Contract;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.NatNegotiation.Handler.CmdHandler
{
    [HandlerContract(RequestType.Ping)]
    public sealed class PingHandler : CmdHandlerBase
    {
        /// <summary>
        /// This is the client pool for storage the clients which traffic need to be redirected
        /// |key: "cookie, clientIndex"| value: Client|
        /// </summary>
        /// <typeparam name="Client"></typeparam>
        /// <returns></returns>
        public static IDictionary<string, Client> TrafficRedirectClientPool = new ConcurrentDictionary<string, Client>();
        public static List<Client> Test = new List<Client>();
        private new PingRequest _request => (PingRequest)base._request;
        private Client _targetClient;
        /// <summary>
        /// The first ping packet will process by natneg server,
        /// when the info is saved, next ping will directly send to another client
        /// </summary>
        /// <param name="client"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public PingHandler(IClient client, IRequest request) : base(client, request)
        {
        }

        protected override void DataOperation()
        {
            // if our client is not in the pool, we will add it
            // if (!TrafficRedirectClientPool.ContainsKey($"{_request.Cookie},{_request.ClientIndex}"))
            // {
            TrafficRedirectClientPool.TryAdd($"{_request.Cookie},{_request.ClientIndex}", _client);
            Test.Add(_client);
            var pp = Client.ClientPool;
            // }
            // we check the other client
            var otherIndex = (NatClientIndex)((int)NatClientIndex.GameServer - _request.ClientIndex);
            Console.WriteLine($"Client index {_request.ClientIndex}-------------------------------------");
            if (TrafficRedirectClientPool.ContainsKey($"{_request.Cookie},{otherIndex}"))
            {
                _targetClient = TrafficRedirectClientPool[$"{_request.Cookie},{otherIndex}"];
            }

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

                if (_targetClient != null)
                {
                    return;
                }
                if (_client.Info.TrafficTransitTarget is null)
                {
                    _client.Info.TrafficTransitTarget = _targetClient;
                    TrafficRedirectClientPool.Remove($"{_request.Cookie},{_request.ClientIndex}");
                }
            }
        }
    }
}