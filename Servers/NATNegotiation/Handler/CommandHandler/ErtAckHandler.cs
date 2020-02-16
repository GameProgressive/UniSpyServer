using System;
using NatNegotiation;
using NATNegotiation.Entity.Structure;

namespace NATNegotiation.Handler.CommandHandler
{
    public class ErtACKHandler
    {
        public ErtACKHandler()
        {
        }

        public void Handle(NatNegServer server, ClientInfo client, byte[] recv)
        {
            client.IsGotErtAck = true;
            throw new NotImplementedException();
        }
    }
}
