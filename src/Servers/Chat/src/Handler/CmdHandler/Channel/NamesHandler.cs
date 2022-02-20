using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;
using UniSpyServer.Servers.Chat.Entity.Structure.Response.Channel;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.Channel;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Handler.CmdHandler.Channel
{
    [HandlerContract("NAMES")]
    public sealed class NamesHandler : ChannelHandlerBase
    {
        private new NamesResult _result { get => (NamesResult)base._result; set => base._result = value; }
        public NamesHandler(IClient client, IRequest request) : base(client, request)
        {
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
