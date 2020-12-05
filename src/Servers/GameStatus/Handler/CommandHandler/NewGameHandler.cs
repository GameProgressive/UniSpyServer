using UniSpyLib.Abstraction.Interface;
using GameStatus.Abstraction.BaseClass;
using System.Collections.Generic;

namespace GameStatus.Handler.CommandHandler
{
    public class NewGameHandler : GSCommandHandlerBase
    {
        // "\newgame\\sesskey\%d\challenge\%d";
        //"\newgame\\connid\%d\sesskey\%d"
        public NewGameHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
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
