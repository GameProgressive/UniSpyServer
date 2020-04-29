using System;
using GameSpyLib.Common.Entity.Interface;

namespace ServerBrowser.Handler.CommandHandler.NatNeg
{
    /// <summary>
    /// we need forward this to game server
    /// </summary>
    public class NatNegCookieHandler:SBCommandHandlerBase
    {
        public NatNegCookieHandler(ISession client, byte[] recv) : base(client, recv)
        {
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();
        }

        protected override void ConstructResponse()
        {
            base.ConstructResponse();
        }

        protected override void DataOperation()
        {
            base.DataOperation();
        }
    }
}
