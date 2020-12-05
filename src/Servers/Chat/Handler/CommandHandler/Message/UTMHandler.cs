using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.Response.Message;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CommandHandler.ChatMessageCommandHandler
{
    public class UTMHandler : ChatMessageHandlerBase
    {
        new UTMRequest _request;

        public UTMHandler(IUniSpySession session, ChatRequestBase request) : base(session, request)
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