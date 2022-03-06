using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;
using UniSpyServer.Servers.GameStatus.Entity.Contract;
using UniSpyServer.Servers.GameStatus.Entity.Exception;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Request;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.GameStatus.Handler.CmdHandler
{
    /// <summary>
    /// Handles game snapshot and update game data
    /// </summary>
    [HandlerContract("updgame")]
    public sealed class UpdateGameHandler : CmdHandlerBase
    {
        //old request "\updgame\\sesskey\%d\done\%d\gamedata\%s"
        //new request "\updgame\\sesskey\%d\connid\%d\done\%d\gamedata\%s"
        private new UpdateGameRequest _request => (UpdateGameRequest)base._request;
        public UpdateGameHandler(IClient client, IRequest request) : base(client, request)
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
            // replace game data with new data
            throw new System.NotImplementedException();
        }
    }
}
