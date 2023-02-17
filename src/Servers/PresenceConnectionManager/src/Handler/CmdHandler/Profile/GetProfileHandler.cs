using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpy.Server.PresenceConnectionManager.Application;
using UniSpy.Server.PresenceConnectionManager.Contract.Request;
using UniSpy.Server.PresenceConnectionManager.Contract.Response;
using UniSpy.Server.PresenceConnectionManager.Contract.Result;
using UniSpy.Server.PresenceSearchPlayer.Exception.General;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.PresenceConnectionManager.Handler.CmdHandler.Profile
{

    public sealed class GetProfileHandler : LoggedInCmdHandlerBase
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

