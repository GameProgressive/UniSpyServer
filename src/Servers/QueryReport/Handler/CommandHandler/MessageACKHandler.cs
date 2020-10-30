using GameSpyLib.Abstraction.Interface;
using System;

namespace QueryReport.Handler.CommandHandler.ClientMessageACK
{
    public class MessageACKHandler : QRCommandHandlerBase
    {
        protected MessageACKHandler(ISession session, byte[] recv) : base(session, recv)
        {
            throw new NotImplementedException();
        }
    }
}
