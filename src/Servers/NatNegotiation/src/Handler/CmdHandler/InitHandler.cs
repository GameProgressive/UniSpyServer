using System;
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
            var count = _redisClient.Values.Count(k =>
            k.Cookie == _request.Cookie
            && k.ClientIndex == _request.ClientIndex
            && k.Version == _request.Version);

            if (count == 4)
            {
                lock (_client.Info)
                {
                    if (_client.Info.IsInitFinish == false)
                    {
                        _client.Info.IsInitFinish = true;
                    }
                    if (_client.Info.IsStartNegotiation == false)
                    {
                        _client.Info.IsStartNegotiation = true;
                        // start send connect packet here
                        Console.WriteLine("Connect 执行了!!!!!!!!!!!!!!!!!!!!!!!!!!! " + _request.PortType.ToString());
                        // var request = new ConnectRequest
                        // {
                        //     PortType = _request.PortType,
                        //     Version = _request.Version,
                        //     Cookie = _request.Cookie,
                        //     ClientIndex = _request.ClientIndex
                        // };
                        // new ConnectHandler(_client, request).Handle();
                    }
                }
                // this means that the client is init already, we can detect the nat type
                var dd = Client.ClientPool.Count;
                return;
            }


        }

        private void UpdateUserInfo()
        {
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

            var count = _redisClient.Values.Count(k =>
              k.ClientIndex == _request.ClientIndex
              & k.Version == _request.Version
              & k.PortType == _request.PortType
              & k.Cookie == _request.Cookie);
            //TODO we get user infomation from redis
            if (count == 0)
            {
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
            }
        }
    }
}
