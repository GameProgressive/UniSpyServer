using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class PRIVMSGHandler : ChatMessageHandlerBase
    {
        new PRIVMSG _cmd;
        public PRIVMSGHandler(ISession client, ChatCommandBase cmd) : base(client, cmd)
        {
            _cmd = (PRIVMSG)cmd;
        }

        public override void DataOperation()
        {
            base.DataOperation();
            switch (_cmd.RequestType)
            {
                case ChatMessageType.ChannelMessage:
                    BuildChannelMessage();
                    break;
                case ChatMessageType.UserMessage:
                    BuildUserMessage();
                    break;
            }
        }
        private void BuildUserMessage()
        {
            _sendingBuffer =
                ChatReply.BuildPrivMsgReply(_user.UserInfo, _cmd.NickName, _cmd.Message);
        }

        private void BuildChannelMessage()
        {
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

            _sendingBuffer =
               ChatReply.BuildPrivMsgReply(_user.UserInfo,
               _cmd.ChannelName, _cmd.Message);
        }
    }
}
