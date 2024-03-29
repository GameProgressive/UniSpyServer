using UniSpy.Server.GameStatus.Abstraction.BaseClass;
using UniSpy.Server.GameStatus.Application;
using UniSpy.Server.GameStatus.Contract.Request;

namespace UniSpy.Server.GameStatus.Handler.CmdHandler
{
    /// <summary>
    /// Create a game specified information storage space
    /// for further game snap shot storage
    /// </summary>

    public sealed class NewGameHandler : CmdHandlerBase
    {
        private new NewGameRequest _request => (NewGameRequest)base._request;
        public NewGameHandler(Client client, NewGameRequest request) : base(client, request)
        {
        }
        protected override void RequestCheck()
        {
            base.RequestCheck();
            _client.Info.GameSessionKey = _request.SessionKey;
        }
        protected override void DataOperation()
        {
            // store game data to database
            StorageOperation.Persistance.CreateNewGameData();
        }
    }
}
