using System.Collections.Generic;

namespace StatsAndTracking.Handler.CommandHandler.NewGame
{
    public class NewGameHandler : CommandHandlerBase
    {
        // "\newgame\\sesskey\%d\challenge\%d";
        //"\newgame\\connid\%d\sesskey\%d"
        protected NewGameHandler(GStatsSession session, Dictionary<string, string> recv) : base(session, recv)
        {
        }
    }
}
