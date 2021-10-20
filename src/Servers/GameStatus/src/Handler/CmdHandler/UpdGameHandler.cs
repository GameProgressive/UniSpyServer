using UniSpyServer.GameStatus.Abstraction.BaseClass;
using UniSpyServer.GameStatus.Entity.Contract;
using UniSpyServer.GameStatus.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.GameStatus.Handler.CmdHandler
{
    /// <summary>
    /// Handles game snapshot and update game data
    /// </summary>
    [HandlerContract("updgame")]
    public sealed class UpdGameHandler : CmdHandlerBase
    {
        //old request "\updgame\\sesskey\%d\done\%d\gamedata\%s"
        //new request "\updgame\\sesskey\%d\connid\%d\done\%d\gamedata\%s"
        public UpdGameHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new UdpGameResult();
        }
        protected override void RequestCheck()
        {
            throw new System.NotImplementedException();
        }

        protected override void DataOperation()
        {
            throw new System.NotImplementedException();
        }

        protected override void ResponseConstruct()
        {
            throw new System.NotImplementedException();
        }
    }
}
