using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.Response;
using Chat.Handler.SystemHandler.ChannelManage;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CommandHandler.ChatGeneralCommandHandler
{
    public class NAMESHandler : ChatCommandHandlerBase
    {
        new NAMESRequest _request;
        ChatChannelBase _channel;
        ChatChannelUser _user;
        public NAMESHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = (NAMESRequest)request;
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();

            if (!ChatChannelManager.GetChannel(_request.ChannelName, out _channel))
            {
                _errorCode = ChatError.NoSuchChannel;
                _sendingBuffer = ChatIRCError.BuildNoSuchChannelError(_request.ChannelName);
            }

            //can not find any user
            if (!_channel.GetChannelUserBySession(_session, out _user))
            {
                _errorCode = ChatError.NoSuchNick;
                _sendingBuffer = ChatIRCError.BuildNoSuchNickError();
                return;
            }

        }

        protected override void Response()
        {
            _channel.SendChannelUsersToJoiner(_user);
        }
    }
}
