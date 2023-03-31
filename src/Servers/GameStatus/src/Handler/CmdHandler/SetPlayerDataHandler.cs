using UniSpy.Server.GameStatus.Abstraction.BaseClass;
using UniSpy.Server.GameStatus.Contract.Request;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.GameStatus.Exception;

namespace UniSpy.Server.GameStatus.Handler.CmdHandler
{
    /// <summary>
    /// Save persist storage data
    /// No response for this handler
    /// </summary>
    public sealed class SetPlayerDataHandler : CmdHandlerBase
    {
        private new SetPlayerDataRequest _request => (SetPlayerDataRequest)base._request;
        public SetPlayerDataHandler(IClient client, IRequest request) : base(client, request)
        {
        }

        protected override void DataOperation()
        {
            throw new GSException("implement set player data");
            // StorageOperation.Persistance.UpdatePlayerData(_request.ProfileId, _request.StorageType, _request.DataIndex, _request.KeyValues);
        }
    }
}
