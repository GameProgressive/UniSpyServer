using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Chat.Contract.Response.General;
using UniSpy.Server.Chat.Contract.Result.General;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Chat.Handler.CmdHandler.General
{
    
    public sealed class PingHandler : LogedInHandlerBase
    {
        private new PingRequest _request => (PingRequest)base._request;
        private new PingResult _result { get => (PingResult)base._result; set => base._result = value; }
        public PingHandler(IChatClient client, PingRequest request) : base(client, request){ }
        protected override void DataOperation()
        {
            _result = new PingResult();
            _result.RequesterIRCPrefix = _client.Info.IRCPrefix;
        }
        protected override void ResponseConstruct()
        {
            _response = new PingResponse(_request, _result);
        }
    }
}
