using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class PRIVMSGHandler : ChatMessageHandlerBase
    {
        new PRIVMSG _request;
        public PRIVMSGHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = (PRIVMSG)request;
        }

        protected override void DataOperation()
        {
            base.DataOperation();
            switch (_request.RequestType)
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
                ChatReply.BuildPrivMsgReply(_session.UserInfo, _request.NickName, _request.Message);
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
               _request.ChannelName, _request.Message);
        }
    }
}
