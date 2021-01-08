using Chat.Entity.Structure;
using Chat.Entity.Structure.Response;
using Chat.Handler.SystemHandler.ChatSessionManage;
using Chat.Network;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Abstraction.BaseClass
{
    public class ChatMsgHandlerBase : ChatChannelHandlerBase
    {
        new ChatMsgRequestBase _request
        {
            get { return (ChatMsgRequestBase)base._request; }
        }
        protected ChatSession _otherSession;

        public ChatMsgHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void RequestCheck()
        {
            switch (_request.RequestType)
            {
                case ChatMessageType.ChannelMessage:
                    base.RequestCheck();
                    break;
                case ChatMessageType.UserMessage:

                    if (_request.RequestType == ChatMessageType.UserMessage)
                    {
                        if (!ChatSessionManager.GetSessionByNickName(_request.NickName, out _otherSession))
                        {
                            _errorCode = ChatErrorCode.NoSuchNick;
                            _sendingBuffer = ChatIRCError.BuildNoSuchNickError();
                        }
                    }
                    break;
                default:
                    _errorCode = ChatErrorCode.Parse;
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
                    _channel.MultiCastExceptSender(_user, _sendingBuffer);
                    break;
                case ChatMessageType.UserMessage:
                    _otherSession.SendAsync(_sendingBuffer);
                    break;
            }

        }
    }
}
