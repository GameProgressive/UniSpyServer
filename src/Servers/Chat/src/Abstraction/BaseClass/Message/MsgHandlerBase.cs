using UniSpyServer.Chat.Abstraction.BaseClass.Message;
using UniSpyServer.Chat.Entity.Exception;
using UniSpyServer.Chat.Entity.Exception.IRC.General;
using UniSpyServer.Chat.Entity.Structure.Misc.ChannelInfo;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Chat.Abstraction.BaseClass
{
    public abstract class MsgHandlerBase : ChannelHandlerBase
    {
        protected new MsgRequestBase _request => (MsgRequestBase)base._request;
        protected new MsgResultBase _result
        {
            get => (MsgResultBase)base._result;
            set => base._result = value;
        }
        protected ChannelUser _reciever;
        public MsgHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void RequestCheck()
        {
            base.RequestCheck();
            switch (_request.MessageType)
            {
                case MessageType.ChannelMessage:
                    base.RequestCheck();
                    break;
                case MessageType.UserMessage:
                    if (_request.MessageType == MessageType.UserMessage)
                    {
                        _reciever = _channel.GetChannelUserByNickName(_request.NickName);
                        if (_reciever == null)
                        {
                            throw new ChatIRCNoSuchNickException(
                                $"No nickname: {_request.NickName} found in channel: {_channel.Property.ChannelName}.");
                        }
                    }
                    break;
                default:
                    throw new Exception("Unknown chat message request type.");
            }
        }
        protected override void DataOperation()
        {
            switch (_request.MessageType)
            {
                case MessageType.ChannelMessage:
                    ChannelMessageDataOpration();
                    break;
                case MessageType.UserMessage:
                    UserMessageDataOperation();
                    break;
            }
        }

        protected virtual void ChannelMessageDataOpration()
        {
            _result.TargetName = _request.ChannelName;
        }
        protected virtual void UserMessageDataOperation()
        {
            _result.TargetName = _request.NickName;
        }
        protected override void Response()
        {
            // response can not be null!
            _response.Build();
            switch (_request.MessageType)
            {
                case MessageType.ChannelMessage:
                    _channel.MultiCastExceptSender(_user, (string)_response.SendingBuffer);
                    break;
                case MessageType.UserMessage:
                    _reciever.UserInfo.Session.SendAsync((string)_response.SendingBuffer);
                    break;
            }
        }
    }
}
