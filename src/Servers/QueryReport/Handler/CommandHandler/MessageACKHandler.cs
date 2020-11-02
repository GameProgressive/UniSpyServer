using UniSpyLib.Abstraction.Interface;
using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Structure.Request;
using System;

namespace QueryReport.Handler.CommandHandler.ClientMessageACK
{
    public class MessageACKHandler : QRCommandHandlerBase
    {
        protected new ClientMessageRequest _request;
        protected MessageACKHandler(ISession session, IRequest request) : base(session, request)
        {
            _request = (ClientMessageRequest)request;
            throw new NotImplementedException();
        }
    }
}
