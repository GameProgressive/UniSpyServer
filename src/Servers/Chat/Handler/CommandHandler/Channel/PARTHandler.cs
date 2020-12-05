using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.ChatCommand;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CommandHandler.ChatChannelCommandHandler
{
    public class PARTHandler : ChatChannelHandlerBase
    {
        new readonly PARTRequest _request;
        public PARTHandler(IUniSpySession session, ChatRequestBase request) : base(session, request)
        {
            _request = (PARTRequest)request;
        }

        protected override void DataOperation()
        {
            base.DataOperation();

            _channel.LeaveChannel(_user, _request.Reason);
        }
    }
}
