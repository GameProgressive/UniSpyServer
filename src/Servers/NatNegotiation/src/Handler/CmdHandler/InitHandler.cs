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
        private UserInfo _userInfo;
        public InitHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new InitResult();
        }
        protected override void DataOperation()
        {
            _userInfo = _redisClient.Values.FirstOrDefault(
                  k => k.ServerID == _client.Session.Server.ServerID
                  & k.RemoteIPEndPoint == _client.Session.RemoteIPEndPoint
                  & k.PortType == _request.PortType
                  & k.Cookie == _request.Cookie);
            //TODO we get user infomation from redis
            if (_userInfo == null)
            {
                _userInfo = new UserInfo()
                {
                    ServerID = _client.Session.Server.ServerID,
                    RemoteIPEndPoint = _client.Session.RemoteIPEndPoint,
                    PortType = _request.PortType,
                    Cookie = (uint)_request.Cookie,
                    LocalIPEndPoint = _request.LocalIPEndPoint,
                    UseGamePort = _request.UseGamePort,
                    ClientIndex = _request.ClientIndex,
                    LastPacketRecieveTime = DateTime.Now
                };
            }
            else
            {
                _userInfo.LastPacketRecieveTime = DateTime.Now;
                _userInfo.RemoteIPEndPoint = _client.Session.RemoteIPEndPoint;
            }
            _result.RemoteIPEndPoint = _client.Session.RemoteIPEndPoint;
            _redisClient.SetValue(_userInfo);
        }

        protected override void ResponseConstruct()
        {
            _response = new InitResponse(_request, _result);
        }
        protected override void Response()
        {
            base.Response();
            var request = new ConnectRequest
            {
                PortType = _request.PortType,
                Version = _request.Version,
                Cookie = _request.Cookie
            };
            new ConnectHandler(_client, request).Handle();
        }
    }
}
