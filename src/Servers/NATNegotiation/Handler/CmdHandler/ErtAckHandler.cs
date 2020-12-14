using UniSpyLib.Abstraction.Interface;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Entity.Structure.Request;
using NATNegotiation.Entity.Structure.Response;
using System;
using NATNegotiation.Entity.Structure;
using NATNegotiation.Handler.SystemHandler.Manager;

namespace NATNegotiation.Handler.CmdHandler
{
    public class ErtAckHandler : NNCommandHandlerBase
    {
        protected new ErtAckRequest _request { get { return (ErtAckRequest)base._request; } }
        public ErtAckHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {

        }

        protected override void DataOperation()
        {
            NatUserInfo userInfo = NegotiatorManager.GetNatUserInfo(_session.RemoteEndPoint, _request.Cookie);
            //TODO we get user infomation from redis
            if (userInfo == null)
            {
                NatUserInfo info = new NatUserInfo();
                info.UpdateRemoteEndPoint(_session.RemoteEndPoint);
                info.UpdateInitRequestInfo(_request);
                info.LastPacketRecieveTime = DateTime.Now;
                NegotiatorManager.SetNatUserInfo(_session.RemoteEndPoint, info.Cookie, info);
            }
        }

        protected override void ConstructResponse()
        {
            _sendingBuffer =
                new ErtAckResponse(_request, _session.RemoteEndPoint).BuildResponse();
        }
    }
}
