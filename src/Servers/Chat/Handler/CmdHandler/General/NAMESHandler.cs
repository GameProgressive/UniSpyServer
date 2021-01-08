using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure;
using Chat.Entity.Structure.ChannelInfo;
using Chat.Entity.Structure.ChannelInfo;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.Response;
using Chat.Handler.SystemHandler.ChannelManage;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.General
{
    public class NAMESHandler : ChatCmdHandlerBase
    {
        protected new NAMESRequest _request { get { return (NAMESRequest)base._request; } }
        private ChatChannel _channel;
        private ChatChannelUser _user;
        public NAMESHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void RequestCheck()
        {
            base.RequestCheck();

            if (!ChatChannelManager.GetChannel(_request.ChannelName, out _channel))
            {
                _errorCode = ChatErrorCode.NoSuchChannel;
                _sendingBuffer = ChatIRCError.BuildNoSuchChannelError(_request.ChannelName);
            }

            //can not find any user
            if (!_channel.GetChannelUserBySession(_session, out _user))
            {
                _errorCode = ChatErrorCode.NoSuchNick;
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
