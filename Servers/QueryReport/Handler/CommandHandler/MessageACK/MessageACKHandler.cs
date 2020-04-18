using GameSpyLib.Common.Entity.Interface;
using System;

namespace QueryReport.Handler.CommandHandler.ClientMessageACK
{
    public class MessageACKHandler : QRCommandHandlerBase
    {
        protected MessageACKHandler(IClient client, byte[] recv) : base(client, recv)
        {
            throw new NotImplementedException();
        }
    }
}
