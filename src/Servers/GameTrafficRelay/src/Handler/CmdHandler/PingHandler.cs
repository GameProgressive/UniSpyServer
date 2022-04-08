using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UniSpyServer.Servers.GameTrafficRelay.Entity.Contract;
using UniSpyServer.Servers.GameTrafficRelay.Entity.Structure.Request;
using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.GameTrafficRelay.Handler.CmdHandler
{
    [HandlerContract(RequestType.Ping)]
    public sealed class PingHandler : CmdHandlerBase
    {
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
            _targetClient = (Client)Client.ClientPool.Values.FirstOrDefault(
                u => ((Client)u).Info.ClientIndex == _request.ClientIndex
                && ((Client)u).Info.Cookie == _request.Cookie);

            Console.WriteLine($"Client index {_request.ClientIndex}-------------------------------------");
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
                if (_client.Info.Cookie is null)
                {
                    _client.Info.Cookie = _request.Cookie;
                }
                if (_targetClient is null)
                {
                    return;
                }
                if (_client.Info.TrafficRelayTarget is null)
                {
                    _client.Info.TrafficRelayTarget = _targetClient;
                }
            }
        }
    }
}