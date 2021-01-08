using UniSpyLib.Abstraction.Interface;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Structure.Request;
using NATNegotiation.Entity.Structure.Response;
using System;
using NATNegotiation.Entity.Structure;

namespace NATNegotiation.Handler.CmdHandler
{
    public class InitHandler : NNCommandHandlerBase
    {
        protected new InitRequest _request { get { return (InitRequest)base._request; } }
        protected NatUserInfo _userInfo;
        protected string _fullKey;
        public InitHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void RequestCheck()
        {
            base.RequestCheck();
            //TODO we get user infomation from redis
            _fullKey = NatUserInfo.RedisOperator.BuildFullKey(
                _session.RemoteIPEndPoint,
                _request.PortType,
                _request.Cookie);
            _userInfo = NatUserInfo.RedisOperator.GetSpecificValue(_fullKey);

            if (_userInfo == null)
            {
                _userInfo = new NatUserInfo();
                _userInfo.UpdateRemoteEndPoint(_session.RemoteIPEndPoint);
            }
        }

        protected override void DataOperation()
        {
            _userInfo.UpdateInitRequestInfo(_request);
            _userInfo.LastPacketRecieveTime = DateTime.Now;
            NatUserInfo.RedisOperator.SetKeyValue(_fullKey, _userInfo);
        }

        protected override void ResponseConstruct()
        {
            _sendingBuffer = new InitResponse(_request, _session.RemoteEndPoint).BuildResponse();
        }

        //protected override void Response()
        //{
        //    base.Response();
        //    NatNegotiateManager
        //        .Negotiate(
        //        _request.PortType,
        //        _request.Version,
        //        _request.Cookie);
        //}
    }
}
