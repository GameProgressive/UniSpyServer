using UniSpyServer.Servers.Chat.Entity.Exception.IRC.Channel;
using UniSpyServer.Servers.Chat.Entity.Exception.IRC.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc.ChannelInfo;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Abstraction.BaseClass
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
    }
}
