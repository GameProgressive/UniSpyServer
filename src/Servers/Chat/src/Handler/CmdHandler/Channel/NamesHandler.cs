using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Error.IRC.Channel;
using UniSpy.Server.Chat.Error.IRC.General;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Chat.Contract.Response.Channel;
using UniSpy.Server.Chat.Contract.Result.Channel;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Chat.Handler.CmdHandler.Channel
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
                if (_channel is null)
                {
                    throw new NoSuchChannelException($"No such channel {_request.ChannelName}", _request.ChannelName);
                }
                _user = _channel.GetChannelUser(_client);
                if (_user is null)
                {
                    throw new NoSuchNickException($"Can not find user with nickname: {_client.Info.NickName} username: {_client.Info.UserName}");
                }
                return;
            }
            base.RequestCheck();
        }
        protected override void DataOperation()
        {
            _result = new NamesResult();
            _result.AllChannelUserNicks = _channel.GetAllUsersNickString();
            _result.ChannelName = _channel.Name;
            _result.RequesterNickName = _user.Info.NickName;
        }
        protected override void ResponseConstruct()
        {
            _response = new NamesResponse(_request, _result);
        }
    }
}
