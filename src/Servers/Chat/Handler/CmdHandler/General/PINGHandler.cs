using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Request.General;
using Chat.Entity.Structure.Response.General;
using Chat.Entity.Structure.Result.General;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.General
{
    internal sealed class PINGHandler : LogedInHandlerBase
    {
        private new PINGRequest _request => (PINGRequest)base._request;
        private new PINGResult _result
        {
            get => (PINGResult)base._result;
            set => base._result = value;
        }
        public PINGHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
        protected override void DataOperation()
        {
            _result.RequesterIRCPrefix = _session.UserInfo.IRCPrefix;
        }
        protected override void ResponseConstruct()
        {
            _response = new PINGResponse(_request, _result);
        }
    }
}
