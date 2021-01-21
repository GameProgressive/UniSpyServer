using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Structure.Request;
using UniSpyLib.Abstraction.Interface;

namespace ServerBrowser.Handler.CmdHandler
{
    internal sealed class SendMessageHandler : SBCmdHandlerBase
    {
        private new AdHocRequest _request => (AdHocRequest)base._request;
        public SendMessageHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void RequestCheck()
        {
        }

        protected override void DataOperation()
        {
            _session.ServerMessageList.Add(_request);
        }

        protected override void ResponseConstruct()
        {
        }
    }
}
