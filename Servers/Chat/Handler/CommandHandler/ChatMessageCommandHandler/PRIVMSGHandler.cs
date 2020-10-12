using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using Chat.Entity.Structure.ChatResponse.ChatMessageResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler.ChatMessageCommandHandler
{
    public class PRIVMSGHandler : ChatMessageHandlerBase
    {
        new PRIVMSGRequest _request;
        public PRIVMSGHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = new PRIVMSGRequest(request.RawRequest);
        }
        protected override void CheckRequest()
        {
            base.CheckRequest();
            if (!_request.Parse())
            {
                _errorCode = ChatError.Parse;
                return;
            }
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
