using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Contract.Request.Message;
using UniSpy.Server.Chat.Contract.Response.Message;
using UniSpy.Server.Chat.Contract.Result.Message;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Chat.Handler.CmdHandler.Message
{
    
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
            _result.UserIRCPrefix = _receiver.Info.IRCPrefix;
        }
        protected override void ResponseConstruct()
        {
            _response = new AboveTheTableMsgResponse(_request, _result);
        }
    }
}
