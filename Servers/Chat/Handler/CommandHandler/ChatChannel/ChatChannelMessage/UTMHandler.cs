using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{

    public class UTMHandler : ChatMessageHandlerBase
    {
        new UTM _request;

        public UTMHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = (UTM)request;
        }

        protected override void DataOperation()
        {
            base.DataOperation();

            switch (_request.RequestType)
            {
                case ChatMessageType.ChannelMessage:
                    _sendingBuffer =
                        ChatReply.BuildUTMReply(_user.UserInfo, _request.ChannelName, _request.Message);
                    break;
                case ChatMessageType.UserMessage:
                    _sendingBuffer =
                        ChatReply.BuildUTMReply(
                        _session.UserInfo, _request.NickName, _request.Message);
                    break;
            }
        }
    }
}
