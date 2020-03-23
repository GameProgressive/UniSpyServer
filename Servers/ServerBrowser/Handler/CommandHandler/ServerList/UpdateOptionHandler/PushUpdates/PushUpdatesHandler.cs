using System;
namespace ServerBrowser.Handler.CommandHandler.ServerList.UpdateOptionHandler.PushUpdates
{
    public class PushUpdatesHandler:CommandHandlerBase
    {
        public PushUpdatesHandler(SBSession session, byte[] recv) : base(session, recv)
        {
        }

        public override void CheckRequest(SBSession session, byte[] recv)
        {
            base.CheckRequest(session, recv);
        }

        public override void DataOperation(SBSession session, byte[] recv)
        {
            base.DataOperation(session, recv);
        }

        public override void ConstructResponse(SBSession session, byte[] recv)
        {
            base.ConstructResponse(session, recv);
        }
    }
}
