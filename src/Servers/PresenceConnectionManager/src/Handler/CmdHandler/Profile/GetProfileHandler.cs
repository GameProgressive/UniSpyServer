using System.Linq;
using UniSpyServer.Servers.PresenceConnectionManager.Application;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Response;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Result;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;

namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.CmdHandler.Profile
{

    public sealed class GetProfileHandler : Abstraction.BaseClass.CmdHandlerBase
    {
        // \getprofile\\sesskey\19150\profileid\2\id\2\final\
        private new GetProfileRequest _request => (GetProfileRequest)base._request;

        private new GetProfileResult _result { get => (GetProfileResult)base._result; set => base._result = value; }

        public GetProfileHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new GetProfileResult();
        }
        protected override void DataOperation()
        {
            var result = StorageOperation.Persistance.GetProfileInfos(_request.ProfileId, _client.Info.SubProfileInfo.NamespaceId);

            if (result is null)
            {
                throw new GPDatabaseException($"No profile of profileid:{_request.ProfileId} found in database.");
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new GetProfileResponse(_request, _result);
        }
    }
}

