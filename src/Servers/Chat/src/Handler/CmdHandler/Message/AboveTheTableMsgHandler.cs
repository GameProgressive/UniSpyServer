using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Contract;
using UniSpyServer.Chat.Entity.Structure.Request.Message;
using UniSpyServer.Chat.Entity.Structure.Response.Message;
using UniSpyServer.Chat.Entity.Structure.Result.Message;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Chat.Handler.CmdHandler.Message
{
    [HandlerContract("ATM")]
    public sealed class AboveTheTableMsgHandler : MsgHandlerBase
    {
        new AboveTheTableMsgRequest _request => (AboveTheTableMsgRequest)base._request;
        new AboveTheTableMsgResult _result
        {
            get => (AboveTheTableMsgResult)base._result;
            set => base._result = value;
        }
        public AboveTheTableMsgHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new AboveTheTableMsgResult();
        }

        protected override void ChannelMessageDataOpration()
        {
            base.ChannelMessageDataOpration();
            _result.UserIRCPrefix = _user.UserInfo.IRCPrefix;
        }
        protected override void UserMessageDataOperation()
        {
            base.UserMessageDataOperation();
            _result.UserIRCPrefix = _reciever.UserInfo.IRCPrefix;
        }
        protected override void ResponseConstruct()
        {
            _response = new AboveTheTableMsgResponse(_request, _result);
        }
    }
}
