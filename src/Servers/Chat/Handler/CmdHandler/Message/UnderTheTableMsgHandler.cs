using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Response.Message;
using Chat.Entity.Structure.Result.Message;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.Message
{
    [HandlerContract("UTM")]
    internal sealed class UnderTheTableMsgHandler : ChatMsgHandlerBase
    {
        private new UnderTheTableMsgRequest _request => (UnderTheTableMsgRequest)base._request;
        private new UTMResult _result
        {
            get => (UTMResult)base._result;
            set => base._result = value;
        }
        public UnderTheTableMsgHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new UTMResult();
        }
        protected override void ChannelMessageDataOpration()
        {
            _result.Name = _request.ChannelName;
            _result.UserIRCPrefix = _user.UserInfo.IRCPrefix;
        }
        protected override void UserMessageDataOperation()
        {
            _result.Name = _reciever.UserInfo.NickName;
            _result.UserIRCPrefix = _reciever.UserInfo.IRCPrefix;
        }
        protected override void ResponseConstruct()
        {
            _response = new UTMResponse(_request, _result);
        }
    }
}