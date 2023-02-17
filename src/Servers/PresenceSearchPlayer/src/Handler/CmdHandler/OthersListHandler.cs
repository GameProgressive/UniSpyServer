using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpy.Server.PresenceSearchPlayer.Exception.General;
using UniSpy.Server.PresenceSearchPlayer.Contract.Request;
using UniSpy.Server.PresenceSearchPlayer.Contract.Response;
using UniSpy.Server.PresenceSearchPlayer.Contract.Result;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.PresenceSearchPlayer.Application;

namespace UniSpy.Server.PresenceSearchPlayer.Handler.CmdHandler
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
