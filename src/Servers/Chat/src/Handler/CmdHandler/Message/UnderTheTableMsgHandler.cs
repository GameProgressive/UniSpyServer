using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.Message;
using UniSpyServer.Servers.Chat.Entity.Structure.Response.Message;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.Message;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Handler.CmdHandler.Message
{
    [HandlerContract("UTM")]
    public sealed class UnderTheTableMsgHandler : MsgHandlerBase
    {
        private new UnderTheTableMsgRequest _request => (UnderTheTableMsgRequest)base._request;
        private new UnderTheTableMsgResult _result { get => (UnderTheTableMsgResult)base._result; set => base._result = value; }
        public UnderTheTableMsgHandler(ISession session, IRequest request) : base(session, request)
        {
            _result = new UnderTheTableMsgResult();
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
            _response = new UnderTheTableMsgResponse(_request, _result);
        }
    }
}