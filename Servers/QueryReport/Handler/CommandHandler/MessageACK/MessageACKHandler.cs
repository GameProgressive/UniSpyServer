using GameSpyLib.Common.Entity.Interface;
using QueryReport.Server;
using System;
using System.Net;

namespace QueryReport.Handler.CommandHandler.ClientMessageACK
{
    public class MessageACKHandler : QRCommandHandlerBase
    {
        protected MessageACKHandler(IClient client, byte[] recv) : base(client,recv)
        {
            throw new NotImplementedException();
        }
    }
}
