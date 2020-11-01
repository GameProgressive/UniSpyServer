using GameSpyLib.Abstraction.Interface;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Entity.Structure.Request;
using NATNegotiation.Entity.Structure.Response;

namespace NATNegotiation.Handler.CommandHandler
{
    public class AddressCheckHandler : NNCommandHandlerBase
    {
        protected new AddressRequest _request;
        public AddressCheckHandler(ISession session, IRequest request) : base(session, request)
        {
            _request = (AddressRequest)request;
        }

        protected override void ConstructResponse()
        {
            _sendingBuffer = new AddressResponse(_request, _session.RemoteEndPoint).BuildResponse();


            _session.UserInfo.SetIsGotAddressCheckPacketFlag().
                UpdateLastPacketReceiveTime();
        }
    }
}
