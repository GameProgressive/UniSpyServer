using UniSpyServer.ServerBrowser.Abstraction.BaseClass;
using UniSpyServer.ServerBrowser.Entity.Structure.Request;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.ServerBrowser.Handler.CmdHandler
{
    public sealed class SendMessageHandler : CmdHandlerBase
    {
        private new AdHocRequest _request => (AdHocRequest)base._request;
        public SendMessageHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            _session.ServerMessageList.Add(_request);
        }
    }
}
