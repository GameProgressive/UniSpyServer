using System;
using ServerBrowser.Entity.Structure;

namespace ServerBrowser.Handler.CommandHandler.PlayerSearch
{
    public class PlayerSearchHandler : SBHandlerBase
    {
        public PlayerSearchHandler(SBSession session, byte[] recv) : base(session, recv)
        {
        }
        private PlayerSearchPacket _packet;
        public override void CheckRequest(SBSession session, byte[] recv)
        {
            base.CheckRequest(session, recv);
            _packet = new PlayerSearchPacket(recv);
        }

        public override void ConstructResponse(SBSession session, byte[] recv)
        {
            base.ConstructResponse(session, recv);
        }

        public override void DataOperation(SBSession session, byte[] recv)
        {
            base.DataOperation(session, recv);
        }
    }
}
