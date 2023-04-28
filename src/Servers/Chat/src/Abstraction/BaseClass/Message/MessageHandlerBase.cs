using System.Linq;
using UniSpy.Server.Chat.Abstraction.BaseClass.Message;
using UniSpy.Server.Chat.Error.IRC.General;
using UniSpy.Server.Chat.Aggregate;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Chat.Application;
using UniSpy.Server.Chat.Abstraction.Interface;

namespace UniSpy.Server.Chat.Abstraction.BaseClass
{
    public abstract class MessageHandlerBase : ChannelHandlerBase
    {
        protected new MessageRequestBase _request => (MessageRequestBase)base._request;
        protected new MessageResultBase _result { get => (MessageResultBase)base._result; set => base._result = value; }
        protected ChannelUser _receiver;
        public MessageHandlerBase(IChatClient client, IRequest request) : base(client, request) { }

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
                    throw new Chat.Exception("Unknown chat message request type.");
            }
        }
        protected virtual void ChannelMessageRequestCheck() => base.RequestCheck();
        protected virtual void UserMessageRequestCheck()
        {
            // todo check if we only allow user join one channel
            // fist we find this user in our local client pool, beacuse nick name is unique, this search is safe
            var client = ClientManager.GetClientByNickName(_request.NickName);
            // we get a first channel in his joined list
            _channel = client.Info.JoinedChannels.Values.First();
            // we find this user in this channel
            _receiver = _channel.GetUser(_request.NickName);
            if (_receiver is null)
            {
                throw new NoSuchNickException(
                    $"No nickname: {_request.NickName} found in channel: {_channel.Name}.");
            }
        }
        protected override void DataOperation()
        {
            _result.UserIRCPrefix = _client.Info.IRCPrefix;
            switch (_request.Type)
            {
                case MessageType.ChannelMessage:
                    _result.TargetName = _request.ChannelName;
                    ChannelMessageDataOpration();
                    break;
                case MessageType.UserMessage:
                    _result.TargetName = _request.NickName;
                    UserMessageDataOperation();
                    break;
            }
        }

        protected virtual void ChannelMessageDataOpration() { }
        protected virtual void UserMessageDataOperation() { }
        protected override void Response()
        {
            // response can not be null!
            switch (_request.Type)
            {
                case MessageType.ChannelMessage:
                    _channel.MultiCast(_user.Client, _response, true);
                    break;
                case MessageType.UserMessage:
                    _receiver.Client.Send(_response);
                    break;
            }
        }
    }
}
