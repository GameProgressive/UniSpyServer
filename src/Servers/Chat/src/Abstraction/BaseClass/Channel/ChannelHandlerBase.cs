using UniSpy.Server.Chat.Exception.IRC.Channel;
using UniSpy.Server.Chat.Exception.IRC.General;
using UniSpy.Server.Chat.Aggregate.Misc.ChannelInfo;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Chat.Application;
using UniSpy.Server.Chat.Aggregate.Redis.Contract;

namespace UniSpy.Server.Chat.Abstraction.BaseClass
{
    public abstract class ChannelHandlerBase : LogedInHandlerBase
    {
        protected Channel _channel;
        protected ChannelUser _user;
        private new ChannelRequestBase _request => (ChannelRequestBase)base._request;
        public ChannelHandlerBase(IClient client, IRequest request) : base(client, request) { }

        protected override void RequestCheck()
        {
            if (_request.RawRequest is not null)
            {
                base.RequestCheck();
            }
            _channel = _client.Info.GetJoinedChannel(_request.ChannelName);
            if (_channel is null)
            {
                throw new ChatIRCNoSuchChannelException($"No such channel {_request.ChannelName}", _request.ChannelName);
            }
            _user = _channel.GetChannelUser(_client);
            if (_user is null)
            {
                throw new ChatIRCNoSuchNickException($"Can not find user with nickname: {_client.Info.NickName} username: {_client.Info.UserName}");
            }
        }
        protected override void PublishMessage()
        {
            var msg = new RemoteMessage(_request, _client.GetRemoteClient());
            _channel.MessageBroker.PublishMessage(msg);
        }
    }
}
