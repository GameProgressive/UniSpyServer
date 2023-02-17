using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Contract.Request.Message;
using UniSpy.Server.Chat.Contract.Response.Message;
using UniSpy.Server.Chat.Contract.Result.Message;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Chat.Handler.CmdHandler.Message
{
    
    public sealed class UnderTheTableMsgHandler : MsgHandlerBase
    {
        private new UnderTheTableMsgRequest _request => (UnderTheTableMsgRequest)base._request;
        private new UnderTheTableMsgResult _result { get => (UnderTheTableMsgResult)base._result; set => base._result = value; }
        public UnderTheTableMsgHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new UnderTheTableMsgResult();
        }
        protected override void ChannelMessageDataOpration()
        {
            _result.Name = _request.ChannelName;
            _result.UserIRCPrefix = _user.Info.IRCPrefix;
        }
        protected override void UserMessageDataOperation()
        {
            _result.Name = _reciever.Info.NickName;
            _result.UserIRCPrefix = _reciever.Info.IRCPrefix;
        }
        protected override void ResponseConstruct()
        {
            _response = new UnderTheTableMsgResponse(_request, _result);
        }
    }
}