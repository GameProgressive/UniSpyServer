using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Structure.Request;
using UniSpyLib.Abstraction.Interface;

namespace ServerBrowser.Handler.CmdHandler
{
    public class SendMessageHandler : SBCmdHandlerBase
    {
        protected new AdHocRequest _request { get { return (AdHocRequest)base._request; } }
        public SendMessageHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            _session.ServerMessageList.Add(_request);
        }

        protected override void RequestCheck()
        {
            throw new System.NotImplementedException();
        }

        protected override void ResponseConstruct()
        {
            throw new System.NotImplementedException();
        }
    }
}
