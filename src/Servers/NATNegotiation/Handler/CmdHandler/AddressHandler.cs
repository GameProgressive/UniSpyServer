using UniSpyLib.Abstraction.Interface;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Structure.Request;
using NATNegotiation.Entity.Structure.Response;
using System;
using NATNegotiation.Entity.Structure;

namespace NATNegotiation.Handler.CmdHandler
{
    public class AddressCheckHandler : NNCommandHandlerBase
    {
        protected new AddressRequest _request { get { return (AddressRequest)base._request; } }
        public AddressCheckHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            base.DataOperation();
            _userInfo.IsGotAddressCheckPacket = true;
            _userInfo.LastPacketRecieveTime = DateTime.Now;
            NatUserInfo.SetNatUserInfo(_session.RemoteEndPoint, _request.Cookie, _userInfo);
        }

        protected override void ConstructResponse()
        {
            _sendingBuffer = new AddressResponse(_request, _session.RemoteEndPoint).BuildResponse();
        }
    }
}
