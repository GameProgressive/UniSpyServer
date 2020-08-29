using Chat.Entity.Structure.ChatCommand;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class PARTHandler : ChatChannelHandlerBase
    {
        new PART _request;
        public PARTHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = (PART)request;
        }

        protected override void DataOperation()
        {
            base.DataOperation();

            _channel.LeaveChannel(_user, _request.Reason);
        }
    }
}
