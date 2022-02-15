using UniSpyServer.Servers.ServerBrowser.Abstraction.BaseClass;
using UniSpyServer.Servers.ServerBrowser.Entity.Contract;
using UniSpyServer.Servers.ServerBrowser.Entity.Enumerate;
using UniSpyServer.Servers.ServerBrowser.Entity.Structure.Request;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.ServerBrowser.Handler.CmdHandler
{
    [HandlerContract(RequestType.SendMessageRequest)]
    public sealed class SendMsgHandler : CmdHandlerBase
    {
        private new SendMessageRequest _request => (SendMessageRequest)base._request;
        public SendMsgHandler(ISession session, IRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            _session.AdHocMessage = _request;
        }
    }
}
