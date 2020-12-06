using UniSpyLib.Abstraction.Interface;
using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Structure.Request;
using System;

namespace QueryReport.Handler.CmdHandler
{
    public class ClientMessageHandler : QRCmdHandlerBase
    {
        protected new ClientMessageRequest _request { get { return (ClientMessageRequest)base._request; } }
        protected ClientMessageHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            throw new NotImplementedException();
        }
    }
}
