using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler.ChatChannelCommandHandler
{
    public class PARTHandler : ChatChannelHandlerBase
    {
        new PARTRequest _request;
        public PARTHandler(ISession session, ChatRequestBase request) : base(session, request)
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
