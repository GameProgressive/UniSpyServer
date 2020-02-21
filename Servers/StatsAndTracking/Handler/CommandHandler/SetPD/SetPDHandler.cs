using System;
using System.Collections.Generic;
using System.Text;

namespace StatsAndTracking.Handler.CommandHandler.SetPD
{
    class SetPDHandler : GStatsHandlerBase
    {
        protected SetPDHandler(GStatsSession session,Dictionary<string, string> recv) : base(session,recv)
        {
        }

        protected override void CheckRequest(GStatsSession session, Dictionary<string, string> recv)
        {
            base.CheckRequest(session,recv);
        }
    }
}
