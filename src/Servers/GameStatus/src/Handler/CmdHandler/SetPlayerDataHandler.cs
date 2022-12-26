using System.Linq;
using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;
using UniSpyServer.Servers.GameStatus.Application;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Request;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;

namespace UniSpyServer.Servers.GameStatus.Handler.CmdHandler
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
