using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;
using UniSpyServer.Servers.GameStatus.Entity.Contract;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Response;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.GameStatus.Handler.CmdHandler
{
    [HandlerContract("newgame")]

    public sealed class CreateNewGameDataHandler : CmdHandlerBase
    {
        // "\newgame\\sesskey\%d\challenge\%d";
        //"\newgame\\connid\%d\sesskey\%d"
        private new CreateNewGameDataResult _result{ get => (CreateNewGameDataResult)base._result; set => base._result = value; }
        public CreateNewGameDataHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new CreateNewGameDataResult();
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
