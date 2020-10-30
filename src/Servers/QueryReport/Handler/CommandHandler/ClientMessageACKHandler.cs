using GameSpyLib.Abstraction.Interface;
using QueryReport.Abstraction.BaseClass;

namespace QueryReport.Handler.CommandHandler.ClientMessage
{
    public class ClientMessageACKHandler : QRCommandHandlerBase
    {
        public ClientMessageACKHandler(ISession session, byte[] recv) : base(session, recv)
        {
        }
    }
}
