using System.Collections.Generic;
using GameSpyLib.Common.Entity.Interface;

namespace StatsAndTracking.Handler.CommandHandler.UpdGame
{
    /// <summary>
    /// Handles game snapshot and update game data
    /// </summary>
    public class UpdGameHandler : GStatsCommandHandlerBase
    {
        //	"\updgame\\sesskey\%d\done\%d\gamedata\%s"
        // or
        // "\updgame\\sesskey\%d\connid\%d\done\%d\gamedata\%s"
        public UpdGameHandler(ISession session, Dictionary<string, string> recv) : base(session, recv)
        {
        }

        protected override void CheckRequest()
        {
            throw new System.NotImplementedException();
        }

        protected override void DataOperation()
        {
            throw new System.NotImplementedException();
        }
    }
}
