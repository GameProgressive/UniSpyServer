using System.Linq;
using UniSpy.Server.Chat.Abstraction.BaseClass.Message;
using UniSpy.Server.Chat.Exception;
using UniSpy.Server.Chat.Exception.IRC.General;
using UniSpy.Server.Chat.Aggregate.Misc.ChannelInfo;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Chat.Abstraction.BaseClass
{
    public abstract class MsgHandlerBase : ChannelHandlerBase
    {
        protected new MsgRequestBase _request => (MsgRequestBase)base._request;
        protected new MsgResultBase _result { get => (MsgResultBase)base._result; set => base._result = value; }
        protected ChannelUser _reciever;
        public MsgHandlerBase(IClient client, IRequest request) : base(client, request){ }

        protected override void RequestCheck()
        {
            // base.RequestCheck();
            _request.Parse();
            switch (_request.Type)
            {
                case MessageType.ChannelMessage:
                    base.RequestCheck();
                    break;
                case MessageType.UserMessage:
                    if (_request.Type == MessageType.UserMessage)
                    {
                        // todo check if we only allow user join one channel
                        _channel = _client.Info.JoinedChannels.Values.First();
                        _reciever = _channel.GetChannelUser(_request.NickName);
                        if (_reciever is null)
                        {
                            throw new ChatIRCNoSuchNickException(
                                $"No nickname: {_request.NickName} found in channel: {_channel.Name}.");
                        }
                    }
                    break;
                default:
                    throw new ChatException("Unknown chat message request type.");
            }
        }
        protected override void DataOperation()
        {
            switch (_request.Type)
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
            switch (_request.Type)
            {
                case MessageType.ChannelMessage:
                    _channel.MultiCastExceptSender(_user, _response);
                    break;
                case MessageType.UserMessage:
                    _reciever.ClientRef.Send(_response);
                    break;
            }
        }
    }
}
