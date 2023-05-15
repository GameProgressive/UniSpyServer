using UniSpy.Server.Chat.Error.IRC.Channel;
using UniSpy.Server.Chat.Error.IRC.General;
using UniSpy.Server.Chat.Aggregate;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Chat.Abstraction.Interface;

namespace UniSpy.Server.Chat.Abstraction.BaseClass
{
    public abstract class ChannelHandlerBase : LogedInHandlerBase
    {
        /// <summary>
        /// The matched channel of chat request
        /// </summary>
        protected Channel _channel;
        /// <summary>
        /// The channel user of current IClient
        /// </summary>
        protected ChannelUser _user;
        private new ChannelRequestBase _request => (ChannelRequestBase)base._request;
        public ChannelHandlerBase(IChatClient client, IRequest request) : base(client, request) { }

        protected override void RequestCheck()
        {
            // we handle join when get user nickname
            if (!_client.Info.IsNickNameSet)
            {
                lock (_client.Info.HandlerStack)
                {
                    _client.Info.HandlerStack.Add(this);
                }
                throw new Chat.HandleLaterException($"{this.GetType().Name} is saved until nickname is set.");
            }
            if (_request.RawRequest is not null)
            {
                base.RequestCheck();
            }
            _channel = _client.Info.GetJoinedChannel(_request.ChannelName);
            if (_channel is null)
            {
                throw new NoSuchChannelException($"No such channel {_request.ChannelName}", _request.ChannelName);
            }
            _user = _channel.GetUser(_client);
            if (_user is null)
            {
                throw new NoSuchNickException($"Can not find user with nickname: {_client.Info.NickName} username: {_client.Info.UserName}");
            }
        }
        protected override void PublishMessage()
        {
            if (_channel is null)
            {
                return;
            }
            base.PublishMessage();
        }
    }
}
