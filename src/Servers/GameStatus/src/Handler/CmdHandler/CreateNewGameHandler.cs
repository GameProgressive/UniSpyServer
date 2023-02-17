using UniSpy.Server.GameStatus.Abstraction.BaseClass;
using UniSpy.Server.GameStatus.Application;
using UniSpy.Server.GameStatus.Entity.Exception;
using UniSpy.Server.GameStatus.Entity.Structure.Request;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.GameStatus.Handler.CmdHandler
{
    /// <summary>
    /// Create a game specified information storage space
    /// for further game snap shot storage
    /// </summary>

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
            StorageOperation.Persistance.CreateNewGameData();
        }
    }
}
