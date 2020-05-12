using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class PRIVMSGHandler : ChatCommandHandlerBase
    {
        new PRIVMSG _cmd;
        private ChatChannelBase _channel;
        private ChatChannelUser _user;
        public PRIVMSGHandler(ISession client, ChatCommandBase cmd) : base(client, cmd)
        {
            _cmd = (PRIVMSG)cmd;
        }

        public override void CheckRequest()
        {
            base.CheckRequest();
            if (!_session.UserInfo.GetJoinedChannelByName(_cmd.ChannelName, out _channel))
            {
                _errorCode = Entity.Structure.ChatError.Parse;
                return;
            }

            if (!_channel.GetChannelUserBySession(_session, out _user))
            {
                _errorCode = Entity.Structure.ChatError.Parse;
                return;
            }
        }

        public override void DataOperation()
        {
            base.DataOperation();
            if (!_channel.Property.ChannelMode.IsModeratedChannel)
            {
                return;
            }

            if (_channel.IsUserBanned(_user))
            {
                return;
            }

            if (!_user.IsVoiceable)
            {
                return;
            }
            if (_user.UserInfo.IsQuietMode)
            {
                return;
            }

        }
        public override void ConstructResponse()
        {
            base.ConstructResponse();
            if (_errorCode > Entity.Structure.ChatError.NoError)
            {
                //
                return;
            }

            BuildNormalMessage();
        }

        public override void Response()
        {
            if (_sendingBuffer == null || _sendingBuffer == "" || _sendingBuffer.Length < 3)
            {
                return;
            }
            _channel.MultiCastExceptSender(_user, _sendingBuffer);
        }

        private void BuildNormalMessage()
        {
            _sendingBuffer =
                ChatReply.BuildPrivMsgReply(_user,
                _channel.Property.ChannelName, _cmd.Message);
        }
    }
}
