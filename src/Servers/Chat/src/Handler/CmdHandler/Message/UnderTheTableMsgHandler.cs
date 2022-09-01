using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.Message;
using UniSpyServer.Servers.Chat.Entity.Structure.Response.Message;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.Message;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Handler.CmdHandler.Message
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