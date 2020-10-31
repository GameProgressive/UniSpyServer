using GameSpyLib.Abstraction.Interface;
using QueryReport.Abstraction.BaseClass;
using System;

namespace QueryReport.Handler.CommandHandler.ClientMessageACK
{
    public class MessageACKHandler : QRCommandHandlerBase
    {
        protected MessageACKHandler(ISession session, byte[] rawRequest) : base(session, rawRequest)
        {
            throw new NotImplementedException();
        }
    }
}
