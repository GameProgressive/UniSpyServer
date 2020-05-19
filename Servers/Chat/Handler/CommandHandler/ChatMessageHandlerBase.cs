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
        new ChatMessageCommandBase _cmd;
        protected ChatSession _otherSession;

        public ChatMessageHandlerBase(ISession session, ChatCommandBase cmd) : base(session, cmd)
        {
            _cmd = (ChatMessageCommandBase)cmd;
        }

        public override void CheckRequest()
        {
            switch (_cmd.RequestType)
            {
                case ChatMessageType.ChannelMessage:
                    base.CheckRequest();
                    break;
                case ChatMessageType.UserMessage:
                    if (_cmd.RequestType == ChatMessageType.UserMessage)
                    {
                        if (!ChatSessionManager.GetSessionByNickName(_cmd.NickName, out _otherSession))
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

        public override void Response()
        {
            if (_sendingBuffer == null || _sendingBuffer == "" || _sendingBuffer.Length < 3)
            {
                return;
            }

            switch (_cmd.RequestType)
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
