using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.ChatCommand;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.Channel
{
    public class PARTHandler : ChatChannelHandlerBase
    {
        protected new  PARTRequest _request { get { return (PARTRequest)base._request; } }
        public PARTHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            base.DataOperation();

            _channel.LeaveChannel(_user, _request.Reason);
        }
    }
}
