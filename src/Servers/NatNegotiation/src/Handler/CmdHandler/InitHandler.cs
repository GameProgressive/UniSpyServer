using System;
using System.Linq;
using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Contract;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
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
            var count = _redisClient.Values.Count(k => k.Cookie == _request.Cookie && k.ClientIndex == _request.ClientIndex);
            if (count == 4)
            {
                // this means that the client is init already, we can detect the nat type
                return;
            }

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
            _userInfo = _redisClient.Values.FirstOrDefault(
                  k => k.ServerID == _client.Session.Server.ServerID
                  & k.PortType == _request.PortType
                  & k.Cookie == _request.Cookie);
            //TODO we get user infomation from redis
            if (_userInfo == null)
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
            }
            else
            {
                _userInfo.PublicIPEndPoint = _client.Session.RemoteIPEndPoint;
            }
            _redisClient.SetValue(_userInfo);
        }
    }
}
