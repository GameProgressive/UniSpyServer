using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;
using UniSpyServer.Servers.GameStatus.Entity.Contract;
using UniSpyServer.Servers.GameStatus.Entity.Exception;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Request;
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
        private new CreateNewGameRequest _request => (CreateNewGameRequest)base._request;
        public CreateNewGameHandler(IClient client, IRequest request) : base(client, request)
        {
        }
        protected override void RequestCheck()
        {
            base.RequestCheck();
            if (_request.SessionKey != _client.Info.SessionKey)
            {
                throw new GSException("Session key is not match");
            }
        }
        protected override void DataOperation()
        {
            // store game data to database
            throw new System.NotImplementedException();
        }
    }
}
