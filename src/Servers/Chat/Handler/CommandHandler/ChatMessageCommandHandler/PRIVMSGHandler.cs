using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse.ChatMessageResponse;
using GameSpyLib.Abstraction.Interface;

namespace Chat.Handler.CommandHandler.ChatMessageCommandHandler
{
    public class PRIVMSGHandler : ChatMessageHandlerBase
    {
        new PRIVMSGRequest _request;
        public PRIVMSGHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = (PRIVMSGRequest)request;
        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
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
                PRIVMSGReply.BuildPrivMsgReply(_session.UserInfo, _request.NickName, _request.Message);
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
               PRIVMSGReply.BuildPrivMsgReply(_user.UserInfo,
               _request.ChannelName, _request.Message);
        }
    }
}
