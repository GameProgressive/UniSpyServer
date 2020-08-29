using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using Chat.Handler.SystemHandler.ChatSessionManage;
using Chat.Server;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class ChatMessageHandlerBase : ChatChannelHandlerBase
    {
        new ChatMessagRequestBase _request;
        protected ChatSession _otherSession;

        public ChatMessageHandlerBase(ISession session, ChatRequestBase cmd) : base(session, cmd)
        {
            _request = (ChatMessagRequestBase)cmd;
        }

        protected override void CheckRequest()
        {
            switch (_request.RequestType)
            {
                case ChatMessageType.ChannelMessage:
                    base.CheckRequest();
                    break;
                case ChatMessageType.UserMessage:
                    if (_request.RequestType == ChatMessageType.UserMessage)
                    {
                        if (!ChatSessionManager.GetSessionByNickName(_request.NickName, out _otherSession))
                        {
                            _errorCode = ChatError.IRCError;
                            _sendingBuffer = ChatIRCError.BuildNoSuchNickError();
                        }
                    }
                    break;
                default:
                    _errorCode = ChatError.Parse;
                    break;
            }
        }

        protected override void Response()
        {
            if (_sendingBuffer == null || _sendingBuffer == "" || _sendingBuffer.Length < 3)
            {
                return;
            }

            switch (_request.RequestType)
            {
                case ChatMessageType.ChannelMessage:
                    _channel.MultiCastExceptSender(_user,_sendingBuffer);
                    break;
                case ChatMessageType.UserMessage:
                    _otherSession.SendAsync(_sendingBuffer);
                    break;
            }

        }
    }
}
