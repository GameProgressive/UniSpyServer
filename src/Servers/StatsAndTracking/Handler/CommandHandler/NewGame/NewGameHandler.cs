using GameSpyLib.Abstraction.Interface;
using StatsTracking.Abstraction.BaseClass;
using System.Collections.Generic;

namespace StatsTracking.Handler.CommandHandler.NewGame
{
    public class NewGameHandler : STCommandHandlerBase
    {
        // "\newgame\\sesskey\%d\challenge\%d";
        //"\newgame\\connid\%d\sesskey\%d"
        public NewGameHandler(ISession session, Dictionary<string, string> request) : base(session, request)
        {
        }

        protected override void CheckRequest()
        {
            throw new System.NotImplementedException();
        }

        protected override void ConstructResponse()
        {
            base.ConstructResponse();
        }

        protected override void DataOperation()
        {
            throw new System.NotImplementedException();
        }
    }
}
