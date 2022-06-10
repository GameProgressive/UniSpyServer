using UniSpyServer.Servers.Chat.Abstraction.BaseClass;

using UniSpyServer.Servers.Chat.Entity.Exception.IRC.Channel;
using UniSpyServer.Servers.Chat.Entity.Exception.IRC.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Response.Channel;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.Channel;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Handler.CmdHandler.Channel
{
    
    public sealed class NamesHandler : ChannelHandlerBase
    {
        private new NamesRequest _request => (NamesRequest)base._request;
        private new NamesResult _result { get => (NamesResult)base._result; set => base._result = value; }
        public NamesHandler(IClient client, IRequest request) : base(client, request) { }
        protected override void RequestCheck()
        {
            if (_request.RawRequest is null)
            {
                _channel = _client.Info.GetJoinedChannel(_request.ChannelName);
                if (_channel == null)
                {
                    throw new ChatIRCNoSuchChannelException($"No such channel {_request.ChannelName}", _request.ChannelName);
                }
                _user = _channel.GetChannelUser(_client);
                if (_user == null)
                {
                    throw new ChatIRCNoSuchNickException($"Can not find user with nickname: {_client.Info.NickName} username: {_client.Info.UserName}");
                }
                return;
            }
            base.RequestCheck();
        }
        protected override void DataOperation()
        {
            _result = new NamesResult();
            _result.AllChannelUserNick = _channel.GetAllUsersNickString();
            _result.ChannelName = _channel.Name;
            _result.RequesterNickName = _user.Info.NickName;
        }
        protected override void ResponseConstruct()
        {
            _response = new NamesResponse(_request, _result);
        }
    }
}
