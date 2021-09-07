using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Structure.Request;
using System;
using UniSpyLib.Abstraction.Interface;

namespace QueryReport.Handler.CmdHandler
{
    internal sealed class ClientMessageHandler : CmdHandlerBase
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
