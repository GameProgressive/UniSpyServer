using GameSpyLib.Abstraction.Interface;

namespace QueryReport.Handler.CommandHandler.ClientMessage
{
    public class ClientMessageACKHandler : QRCommandHandlerBase
    {
        public ClientMessageACKHandler(ISession session, byte[] recv) : base(session, recv)
        {
        }
    }
}
