using GameSpyLib.Abstraction.Interface;
using QueryReport.Abstraction.BaseClass;

namespace QueryReport.Handler.CommandHandler.ClientMessage
{
    public class ClientMessageACKHandler : QRCommandHandlerBase
    {
        public ClientMessageACKHandler(ISession session,IRequest request) : base(session, request)
        {
        }
    }
}
