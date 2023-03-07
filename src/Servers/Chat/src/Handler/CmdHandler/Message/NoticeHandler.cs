using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Contract.Request.Message;
using UniSpy.Server.Chat.Contract.Response.Message;
using UniSpy.Server.Chat.Contract.Result.Message;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Chat.Handler.CmdHandler.Message
{
    
    public sealed class NoticeHandler : MessageHandlerBase
    {
        private new NoticeRequest _request => (NoticeRequest)base._request;
        private new NoticeResult _result{ get => (NoticeResult)base._result; set => base._result = value; }
        public NoticeHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new NoticeResult();
        }

        protected override void ResponseConstruct()
        {
            _response = new NoticeResponse(_request, _result);
        }
    }
}



