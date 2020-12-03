using UniSpyLib.Abstraction.Interface;
using GameStatus.Abstraction.BaseClass;
using System.Collections.Generic;

namespace GameStatus.Handler.CommandHandler
{
    /// <summary>
    /// Handles game snapshot and update game data
    /// </summary>
    public class UpdGameHandler : GSCommandHandlerBase
    {
        //old request "\updgame\\sesskey\%d\done\%d\gamedata\%s"
        //new request "\updgame\\sesskey\%d\connid\%d\done\%d\gamedata\%s"
        public UpdGameHandler(ISession session, IRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            throw new System.NotImplementedException();
        }
    }
}
