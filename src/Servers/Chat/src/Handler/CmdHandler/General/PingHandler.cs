using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Response.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.General;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Handler.CmdHandler.General
{
    [HandlerContract("PING")]
    public sealed class PingHandler : LogedInHandlerBase
    {
        private new PingRequest _request => (PingRequest)base._request;
        private new PingResult _result { get => (PingResult)base._result; set => base._result = value; }
        public PingHandler(IClient client, IRequest request) : base(client, request){ }
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
