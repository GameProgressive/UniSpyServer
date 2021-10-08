using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;
using Chat.Entity.Structure.Response.Channel;
using Chat.Entity.Structure.Result.Channel;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.Channel
{
    [HandlerContract("NAMES")]
    internal sealed class NamesHandler : ChannelHandlerBase
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
