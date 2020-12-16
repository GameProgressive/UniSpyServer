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
        public InitHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
        protected override void CheckRequest()
        {
            base.CheckRequest();
            //TODO we get user infomation from redis
            var keys = NatUserInfo.GetMatchedKeys(_session.RemoteEndPoint, _request.PortType, _request.Cookie);
            if (keys.Count == 0)
            {
                _userInfo = new NatUserInfo();
                _userInfo.UpdateRemoteEndPoint(_session.RemoteEndPoint);
            }
            else if (keys.Count != 1)
            {
                _errorCode = Entity.Enumerate.NNErrorCode.InitPacketError;
                return;
            }
            else
            {
                _userInfo = NatUserInfo.GetNatUserInfo(_session.RemoteEndPoint, _request.PortType, _request.Cookie);
            }
        }

        protected override void DataOperation()
        {
            _userInfo.UpdateInitRequestInfo(_request);
            _userInfo.LastPacketRecieveTime = DateTime.Now;
            NatUserInfo.SetNatUserInfo(_session.RemoteEndPoint, _request.PortType, _userInfo.Cookie, _userInfo);
        }

        protected override void ConstructResponse()
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
