using UniSpyLib.Abstraction.Interface;
using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Structure.Request;
using System;

namespace QueryReport.Handler.CommandHandler
{
    public class ClientMessageHandler : QRCommandHandlerBase
    {
        protected new ClientMessageRequest _request;
        protected ClientMessageHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _request = (ClientMessageRequest)request;
            throw new NotImplementedException();
        }
    }
}
