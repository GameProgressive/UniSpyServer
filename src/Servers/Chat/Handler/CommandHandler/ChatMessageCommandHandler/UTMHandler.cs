using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse.ChatMessageResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler.ChatMessageCommandHandler
{
    public class UTMHandler : ChatMessageHandlerBase
    {
        new UTMRequest _request;

        public UTMHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = (UTMRequest)request;
        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
            switch (_request.RequestType)
            {
                case ChatMessageType.ChannelMessage:
                    _sendingBuffer =
                        UTMReply.BuildUTMReply(_user.UserInfo, _request.ChannelName, _request.Message);
                    break;
                case ChatMessageType.UserMessage:
                    _sendingBuffer =
                        UTMReply.BuildUTMReply(
                        _session.UserInfo, _request.NickName, _request.Message);
                    break;
            }
        }
    }
}