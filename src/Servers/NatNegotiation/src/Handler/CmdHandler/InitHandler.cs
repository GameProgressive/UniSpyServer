using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Application;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Redis;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Response;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Result;
using System;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.Servers.NatNegotiation.Entity.Contract;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using System.Linq;

namespace UniSpyServer.Servers.NatNegotiation.Handler.CmdHandler
{
    [HandlerContract(RequestType.Init)]
    public sealed class InitHandler : CmdHandlerBase
    {
        private new InitRequest _request => (InitRequest)base._request;
        private new InitResult _result { get => (InitResult)base._result; set => base._result = value; }
        private UserInfo _userInfo;
        public InitHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new InitResult();
        }
        protected override void DataOperation()
        {
            _userInfo = _redisClient.Values.Where(
                  k => k.ServerID == ServerFactory.Server.ServerID
                  & k.RemoteIPEndPoint == _session.RemoteIPEndPoint
                  & k.PortType == _request.PortType
                  & k.Cookie == _request.Cookie).FirstOrDefault();
            //TODO we get user infomation from redis
            if (_userInfo == null)
            {
                _userInfo = new UserInfo()
                {
                    ServerID = ServerFactory.Server.ServerID,
                    RemoteIPEndPoint = _session.RemoteIPEndPoint,
                    PortType = _request.PortType,
                    Cookie = _request.Cookie,
                    RequestInfo = _request,
                    LastPacketRecieveTime = DateTime.Now
                };
            }
            else
            {
                _userInfo.LastPacketRecieveTime = DateTime.Now;
                _userInfo.RemoteIPEndPoint = _session.RemoteIPEndPoint;
            }
            _result.RemoteIPEndPoint = _session.RemoteIPEndPoint;
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
            new ConnectHandler(_session, request).Handle();
        }
    }
}
