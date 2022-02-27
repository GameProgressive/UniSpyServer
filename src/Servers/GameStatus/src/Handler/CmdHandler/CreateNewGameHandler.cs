using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;
using UniSpyServer.Servers.GameStatus.Entity.Contract;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Request;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Response;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.GameStatus.Handler.CmdHandler
{
    /// <summary>
    /// Create a game specified information storage space
    /// for further game snap shot storage
    /// </summary>
    [HandlerContract("newgame")]
    public sealed class CreateNewGameHandler : CmdHandlerBase
    {
        // "\newgame\\sesskey\%d\challenge\%d";
        //"\newgame\\connid\%d\sesskey\%d"
        private new CreateNewGameResult _result { get => (CreateNewGameResult)base._result; set => base._result = value; }
        private new CreateNewGameRequest _request => (CreateNewGameRequest)base._request;
        public CreateNewGameHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new CreateNewGameResult();
        }

        protected override void DataOperation()
        {
            throw new System.NotImplementedException();
        }
        protected override void ResponseConstruct()
        {
            _response = new CreateNewGameResponse(_request, _result);
        }
    }
}
