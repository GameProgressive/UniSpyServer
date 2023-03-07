using System.Linq;
using UniSpy.Server.Chat.Abstraction.BaseClass.Message;
using UniSpy.Server.Chat.Exception;
using UniSpy.Server.Chat.Exception.IRC.General;
using UniSpy.Server.Chat.Aggregate.Misc.ChannelInfo;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Chat.Application;

namespace UniSpy.Server.Chat.Abstraction.BaseClass
{
    public abstract class MessageHandlerBase : ChannelHandlerBase
    {
        protected new MessageRequestBase _request => (MessageRequestBase)base._request;
        protected new MessageResultBase _result { get => (MessageResultBase)base._result; set => base._result = value; }
        protected ChannelUser _receiver;
        public MessageHandlerBase(IClient client, IRequest request) : base(client, request) { }

        protected override void RequestCheck()
        {
            _request.Parse();
            switch (_request.Type)
            {
                case MessageType.ChannelMessage:
                    ChannelMessageRequestCheck();
                    break;
                case MessageType.UserMessage:
                    UserMessageRequestCheck();
                    break;
                default:
                    throw new ChatException("Unknown chat message request type.");
            }
        }
        protected virtual void ChannelMessageRequestCheck()
        {
            base.RequestCheck();
        }
        protected virtual void UserMessageRequestCheck()
        {
            // todo check if we only allow user join one channel
            // fist we find this user in our local client pool, beacuse nick name is unique, this search is safe
            var client = ClientManager.GetClientByNickName(_request.NickName);
            // we get a first channel in his joined list
            _channel = client.Info.JoinedChannels.Values.First();
            // we find this user in this channel
            _receiver = _channel.GetChannelUser(_request.NickName);
            if (_receiver is null)
            {
                throw new ChatIRCNoSuchNickException(
                    $"No nickname: {_request.NickName} found in channel: {_channel.Name}.");
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
            _result.UserIRCPrefix = _user.Info.IRCPrefix;
        }
        protected virtual void UserMessageDataOperation()
        {
            _result.TargetName = _request.NickName;
            _result.UserIRCPrefix = _receiver.Info.IRCPrefix;
        }
        protected override void Response()
        {
            // response can not be null!
            switch (_request.Type)
            {
                case MessageType.ChannelMessage:
                    _channel.MultiCast(_user.ClientRef, _response, true);
                    break;
                case MessageType.UserMessage:
                    _receiver.ClientRef.Send(_response);
                    break;
            }
        }
    }
}
