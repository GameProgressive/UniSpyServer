using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Request.General;
using Chat.Entity.Structure.Result.Channel;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.Channel
{
    internal sealed class NAMESHandler : ChatChannelHandlerBase
    {
        private new NAMESResult _result
        {
            get => (NAMESResult)base._result;
            set => base._result = value;
        }
        public NAMESHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
        protected override void DataOperation()
        {
            _result.ChannelUserNickString = _channel.GetAllUsersNickString();
            _result.ChannelName = _channel.Property.ChannelName;
            _result.RequesterNickName = _user.UserInfo.NickName;
        }
        protected override void Response()
        {
            _response.Build();
            _session.SendAsync((string)_response.SendingBuffer);
        }
    }
}
