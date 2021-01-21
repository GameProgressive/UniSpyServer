using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Request;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.Channel
{
    internal sealed class PARTHandler : ChatChannelHandlerBase
    {
        private new PARTRequest _request => (PARTRequest)base._request;
        public PARTHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            _channel.LeaveChannel(_user, _request.Reason);
        }

        protected override void ResponseConstruct()
        {
        }
    }
}
