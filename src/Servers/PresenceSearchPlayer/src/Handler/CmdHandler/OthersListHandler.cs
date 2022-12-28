using UniSpyServer.Servers.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Request;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Response;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Result;
using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;
using UniSpyServer.Servers.PresenceSearchPlayer.Application;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Handler.CmdHandler
{


    public sealed class OthersListHandler : CmdHandlerBase
    {
        private new OthersListRequest _request => (OthersListRequest)base._request;

        private new OthersListResult _result { get => (OthersListResult)base._result; set => base._result = value; }

        public OthersListHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new OthersListResult();
        }

        protected override void DataOperation()
        {
            try
            {
                _result.DatabaseResults = StorageOperation.Persistance.GetMatchedProfileIdInfos(_request.ProfileIDs, _request.NamespaceID);
            }
            catch (System.Exception e)
            {
                throw new GPDatabaseException("Unknown error occurs in database operation.", e);
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new OthersListResponse(_request, _result);
        }
    }
}
