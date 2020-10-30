using GameSpyLib.Abstraction.Interface;
using StatsAndTracking.Abstraction.BaseClass;
using System.Collections.Generic;

namespace StatsAndTracking.Handler.CommandHandler.UpdGame
{
    /// <summary>
    /// Handles game snapshot and update game data
    /// </summary>
    public class UpdGameHandler : GStatsCommandHandlerBase
    {
        //old request "\updgame\\sesskey\%d\done\%d\gamedata\%s"
        //new request "\updgame\\sesskey\%d\connid\%d\done\%d\gamedata\%s"
        public UpdGameHandler(ISession session, Dictionary<string, string> request) : base(session, request)
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
