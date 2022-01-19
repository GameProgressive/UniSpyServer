using UniSpyServer.Servers.ServerBrowser.Abstraction.BaseClass;
using UniSpyServer.Servers.ServerBrowser.Entity.Structure.Request;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.ServerBrowser.Handler.CmdHandler
{
    public sealed class SendMessageHandler : CmdHandlerBase
    {
        private new AdHocRequest _request => (AdHocRequest)base._request;
        public SendMessageHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            _session.ServerMessageStack.Add(_request);
        }
    }
}
