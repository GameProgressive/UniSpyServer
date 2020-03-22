using System.Collections.Generic;

namespace StatsAndTracking.Handler.CommandHandler.UpdGame
{
    /// <summary>
    /// Handles game snapshot and update game data
    /// </summary>
    public class UpdGameHandler : CommandHandlerBase
    {
        //	"\updgame\\sesskey\%d\done\%d\gamedata\%s"
        // or
        // "\updgame\\sesskey\%d\connid\%d\done\%d\gamedata\%s"
        public UpdGameHandler(GStatsSession session, Dictionary<string, string> recv) : base(session, recv)
        {
        }
    }
}
