using UniSpy.Server.GameStatus.Abstraction.BaseClass;
using UniSpy.Server.GameStatus.Application;
using UniSpy.Server.GameStatus.Entity.Structure.Request;
using UniSpy.Server.Core.Abstraction.Interface;

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
            StorageOperation.Persistance.UpdatePlayerData(_request.ProfileId, _request.StorageType, _request.DataIndex, _request.PlayerData);
        }
    }
}
