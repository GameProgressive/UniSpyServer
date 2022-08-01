using UniSpyServer.Servers.Chat.Abstraction.BaseClass;

using UniSpyServer.Servers.Chat.Entity.Structure.Request.Message;
using UniSpyServer.Servers.Chat.Entity.Structure.Response.Message;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.Message;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Handler.CmdHandler.Message
{
    
    public sealed class NoticeHandler : MsgHandlerBase
    {
        private new NoticeRequest _request => (NoticeRequest)base._request;
        private new NoticeResult _result{ get => (NoticeResult)base._result; set => base._result = value; }
        public NoticeHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new NoticeResult();
        }

        protected override void DataOperation()
        {
            _result.UserIRCPrefix = _user.Info.IRCPrefix;
            base.DataOperation();
        }

        protected override void ResponseConstruct()
        {
            _response = new NoticeResponse(_request, _result);
        }
    }
}



