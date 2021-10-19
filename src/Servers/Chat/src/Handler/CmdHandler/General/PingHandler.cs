using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;
using Chat.Entity.Structure.Request.General;
using Chat.Entity.Structure.Response.General;
using Chat.Entity.Structure.Result.General;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.General
{
    [HandlerContract("PING")]
    public sealed class PingHandler : LogedInHandlerBase
    {
        private new PingRequest _request => (PingRequest)base._request;
        private new PingResult _result
        {
            get => (PingResult)base._result;
            set => base._result = value;
        }
        public PingHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
        protected override void DataOperation()
        {
            _result.RequesterIRCPrefix = _session.UserInfo.IRCPrefix;
        }
        protected override void ResponseConstruct()
        {
            _response = new PingResponse(_request, _result);
        }
    }
}
