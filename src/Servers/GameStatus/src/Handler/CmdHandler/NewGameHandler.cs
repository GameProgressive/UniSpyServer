using UniSpyServer.GameStatus.Abstraction.BaseClass;
using UniSpyServer.GameStatus.Entity.Contract;
using UniSpyServer.GameStatus.Entity.Structure.Response;
using UniSpyServer.GameStatus.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.GameStatus.Handler.CmdHandler
{
    [HandlerContract("newgame")]

    public sealed class NewGameHandler : CmdHandlerBase
    {
        // "\newgame\\sesskey\%d\challenge\%d";
        //"\newgame\\connid\%d\sesskey\%d"
        private new NewGameResult _result
        {
            get => (NewGameResult)base._result;
            set => base._result = value;
        }
        public NewGameHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new NewGameResult();
        }

        protected override void DataOperation()
        {
            throw new System.NotImplementedException();
        }
        protected override void ResponseConstruct()
        {
            _response = new NewGameResponse(_request, _result);
        }
    }
}
