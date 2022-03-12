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
        private static List<InitResult> _results;
        private new InitRequest _request => (InitRequest)base._request;
        private new InitResult _result { get => (InitResult)base._result; set => base._result = value; }
        static InitHandler()
        {
            _results = new List<InitResult>();
        }
        public InitHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new InitResult();
        }
        protected override void DataOperation()
        {
            _result.PublicIPEndPoint = _client.Session.RemoteIPEndPoint;
            _result.PrivateIPEndPoint = _request.PrivateIPEndPoint;
            _result.PortType = (NatPortType)_request.PortType;
            _result.Cookie = _request.Cookie;
            _result.ClientIndex = _request.ClientIndex;
            _results.Add(_result);
        }
        protected override void ResponseConstruct()
        {
            _response = new InitResponse(_request, _result);
        }
        protected override void Response()
        {

            base.Response();
            if (_results.Count(r => r.ClientIndex == _request.ClientIndex && r.Cookie == _request.Cookie) == 4)
            {
                return;
            }
            // if (_client.Info.InitResults.Values.Count == 4)
            // {
            //     return;
            // }
            // if (_client.Info.IsInitFinished)
            // {
            //     var request = new ConnectRequest
            //     {
            //         PortType = _request.PortType,
            //         Version = _request.Version,
            //         Cookie = _request.Cookie
            //     };
            //     new ConnectHandler(_client, request).Handle();
            // }
            // UpdateUserInfo();
            // var request = new ConnectRequest
            // {
            //     PortType = _request.PortType,
            //     Version = _request.Version,
            //     Cookie = _request.Cookie
            // };
            // new ConnectHandler(_client, request).Handle();
        }

        private void UpdateUserInfo()
        {
            // _userInfo = _redisClient.Values.FirstOrDefault(
            //       k => k.ServerID == _client.Session.Server.ServerID
            //       & k.RemoteIPEndPoint == _client.Session.RemoteIPEndPoint
            //       & k.PortType == _request.PortType
            //       & k.Cookie == _request.Cookie);
            // //TODO we get user infomation from redis
            // if (_userInfo == null)
            // {
            //     _userInfo = new NatUserInfo()
            //     {
            //         ServerID = _client.Session.Server.ServerID,
            //         RemoteIPEndPoint = _client.Session.RemoteIPEndPoint,
            //         PortType = _request.PortType,
            //         Cookie = (uint)_request.Cookie,
            //         LocalIPEndPoint = _request.LocalIPEndPoint,
            //         UseGamePort = _request.UseGamePort,
            //         ClientIndex = _request.ClientIndex,
            //         LastPacketRecieveTime = DateTime.Now
            //     };
            // }
            // else
            // {
            //     _userInfo.LastPacketRecieveTime = DateTime.Now;
            //     _userInfo.RemoteIPEndPoint = _client.Session.RemoteIPEndPoint;
            // }
            // _redisClient.SetValue(_userInfo);
        }
    }
}
