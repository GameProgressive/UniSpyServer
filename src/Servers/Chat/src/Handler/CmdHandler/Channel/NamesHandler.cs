using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Contract;
using UniSpyServer.Chat.Entity.Structure.Response.Channel;
using UniSpyServer.Chat.Entity.Structure.Result.Channel;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Chat.Handler.CmdHandler.Channel
{
    [HandlerContract("NAMES")]
    public sealed class NamesHandler : ChannelHandlerBase
    {
        private new NamesResult _result { get => (NamesResult)base._result; set => base._result = value; }
        public NamesHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new NamesResult();
        }
        protected override void DataOperation()
        {
            _result.AllChannelUserNick = _channel.GetAllUsersNickString();
            _result.ChannelName = _channel.Property.ChannelName;
            _result.RequesterNickName = _user.UserInfo.NickName;
        }
        protected override void ResponseConstruct()
        {
            _response = new NamesResponse(_request, _result);
        }
    }
}
