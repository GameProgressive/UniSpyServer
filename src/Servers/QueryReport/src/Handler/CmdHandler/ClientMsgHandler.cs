using UniSpyServer.Servers.QueryReport.Abstraction.BaseClass;
using UniSpyServer.Servers.QueryReport.Entity.contract;
using UniSpyServer.Servers.QueryReport.Entity.Enumerate;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Request;
using System;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.QueryReport.Handler.CmdHandler
{
    [HandlerContract(RequestType.ClientMessage)]
    public sealed class ClientMessageHandler : CmdHandlerBase
    {
        private new ClientMessageRequest _request => (ClientMessageRequest)base._request;
        public ClientMessageHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void RequestCheck()
        {
            throw new NotImplementedException();
        }

        protected override void DataOperation()
        {
            throw new NotImplementedException();
        }

        protected override void ResponseConstruct()
        {
            throw new NotImplementedException();
        }
    }
}
