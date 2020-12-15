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
        public InitHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            _userInfo.UpdateInitRequestInfo(_request);
            _userInfo.LastPacketRecieveTime = DateTime.Now;
            NatUserInfo.SetNatUserInfo(_session.RemoteEndPoint, _userInfo.Cookie, _userInfo);
        }

        protected override void ConstructResponse()
        {
            _sendingBuffer = new InitResponse(_request, _session.RemoteEndPoint).BuildResponse();

            //_request.CommandName = NatPacketType.InitAck;
            //_request.BuildResponse();
        }

        //protected override void Response()
        //{
        //    base.Response();
        //    NegotiatorManager
        //        .Negotiate(
        //        _request.PortType,
        //        _request.Version,
        //        _request.Cookie);
        //}
    }
}
