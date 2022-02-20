using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.Message;
using UniSpyServer.Servers.Chat.Entity.Structure.Response.Message;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.Message;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Handler.CmdHandler.Message
{
    [HandlerContract("ATM")]
    public sealed class AboveTheTableMsgHandler : MsgHandlerBase
    {
        new AboveTheTableMsgRequest _request => (AboveTheTableMsgRequest)base._request;
        new AboveTheTableMsgResult _result{ get => (AboveTheTableMsgResult)base._result; set => base._result = value; }
        public AboveTheTableMsgHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new AboveTheTableMsgResult();
        }

        protected override void ChannelMessageDataOpration()
        {
            base.ChannelMessageDataOpration();
            _result.UserIRCPrefix = _user.Info.IRCPrefix;
        }
        protected override void UserMessageDataOperation()
        {
            base.UserMessageDataOperation();
            _result.UserIRCPrefix = _reciever.Info.IRCPrefix;
        }
        protected override void ResponseConstruct()
        {
            _response = new AboveTheTableMsgResponse(_request, _result);
        }
    }
}
