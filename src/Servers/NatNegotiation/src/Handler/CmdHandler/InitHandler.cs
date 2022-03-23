using System;
using System.Collections.Generic;
using System.Linq;
using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Contract;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Redis;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Response;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.NatNegotiation.Handler.CmdHandler
{
    [HandlerContract(RequestType.Init)]
    public sealed class InitHandler : CmdHandlerBase
    {
        private new InitRequest _request => (InitRequest)base._request;
        private new InitResult _result { get => (InitResult)base._result; set => base._result = value; }
        private NatInitInfo _userInfo;

        public InitHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new InitResult();
        }
        protected override void DataOperation()
        {
            _result.RemoteIPEndPoint = _client.Session.RemoteIPEndPoint;
        }
        protected override void ResponseConstruct()
        {
            _response = new InitResponse(_request, _result);
        }
        protected override void Response()
        {
            base.Response();

            UpdateUserInfo();
            // we check if all init packet is received
            var initCount = _redisClient.Values.Count(k =>
            k.Cookie == _request.Cookie
            && k.Version == _request.Version);
            // if there are 8 init result in redis, Client.Pool must have the information of both client
            // we just find it from Client.Pool based on the initResult when we are in ConnectHandler 

            if (initCount == 8)
            {
                lock (ConnectHandler.ConnectStatus)
                {
                    if (!ConnectHandler.ConnectStatus.ContainsKey((uint)_request.Cookie))
                    {
                        ConnectHandler.ConnectStatus.Add((uint)_request.Cookie, true);
                        Console.WriteLine("Connect 执行了!!!!!!!!!!!!!!!!!!!!!!!!!!! " + _request.PortType.ToString());
                        // we start sending connect packet to both of the clients
                        StartConnecting();
                    }
                }
            }
        }
        private void StartConnecting()
        {
            var request = new ConnectRequest
            {
                PortType = _request.PortType,
                Version = _request.Version,
                Cookie = _request.Cookie,
                ClientIndex = _request.ClientIndex
            };
            new ConnectHandler(_client, request).Handle();
        }
        private void UpdateUserInfo()
        {
            // we just update the information on redis
            _userInfo = new NatInitInfo()
            {
                ServerID = _client.Session.Server.ServerID,
                PublicIPEndPoint = _client.Session.RemoteIPEndPoint,
                PortType = _request.PortType,
                Cookie = (uint)_request.Cookie,
                PrivateIPEndPoint = _request.PrivateIPEndPoint,
                UseGamePort = _request.UseGamePort,
                ClientIndex = (NatClientIndex)_request.ClientIndex,
                Version = _request.Version
            };
            _redisClient.SetValue(_userInfo);

            // because the init packet is send with small interval,
            // we do not need to refresh the expire time in redis
            lock (_client.Info)
            {
                if (_client.Info.ClientIndex is null)
                {
                    _client.Info.ClientIndex = _request.ClientIndex;
                }
                if (_client.Info.Cookie is null)
                {
                    _client.Info.Cookie = _request.Cookie;
                }
            }
        }
    }
}
