using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpy.Server.PresenceSearchPlayer.Entity.Exception.General;
using UniSpy.Server.PresenceSearchPlayer.Entity.Structure.Request;
using UniSpy.Server.PresenceSearchPlayer.Entity.Structure.Response;
using UniSpy.Server.PresenceSearchPlayer.Entity.Structure.Result;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.PresenceSearchPlayer.Application;

namespace UniSpy.Server.PresenceSearchPlayer.Handler.CmdHandler
{

    /// <summary>
    /// Get buddy's information
    /// </summary>

    public sealed class OthersHandler : CmdHandlerBase
    {
        private new OthersRequest _request => (OthersRequest)base._request;

        private new OthersResult _result { get => (OthersResult)base._result; set => base._result = value; }
        public OthersHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new OthersResult();
        }

        protected override void DataOperation()
        {
            try
            {

                _result.DatabaseResults = StorageOperation.Persistance.GetFriendsInfo(_request.ProfileId, _request.NamespaceID, _request.GameName);
            }
            catch (System.Exception e)
            {
                throw new GPDatabaseException("Unknown error occurs in database operation.", e);
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new OthersResponse(_request, _result);
        }
    }
}
