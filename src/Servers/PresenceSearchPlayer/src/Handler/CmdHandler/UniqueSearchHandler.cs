using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpy.Server.PresenceSearchPlayer.Exception.General;
using UniSpy.Server.PresenceSearchPlayer.Contract.Request;
using UniSpy.Server.PresenceSearchPlayer.Contract.Response;
using UniSpy.Server.PresenceSearchPlayer.Contract.Result;
using UniSpy.Server.PresenceSearchPlayer.Application;

namespace UniSpy.Server.PresenceSearchPlayer.Handler.CmdHandler
{

    public sealed class UniqueSearchHandler : CmdHandlerBase
    {
        private new UniqueSearchRequest _request => (UniqueSearchRequest)base._request;
        private new UniqueSearchResult _result { get => (UniqueSearchResult)base._result; set => base._result = value; }
        public UniqueSearchHandler(Client client, UniqueSearchRequest request) : base(client, request)
        {
            _result = new UniqueSearchResult();
        }
        protected override void DataOperation()
        {
            try
            {
                _result.IsUniquenickExist = StorageOperation.Persistance.IsUniqueNickExist(_request.PreferredNick,
                                                                           _request.NamespaceID,
                                                                           _request.GameName);
            }
            catch (System.Exception e)
            {
                throw new GPDatabaseException("Unknown error occurs in database operation.", e);
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new UniqueSearchResponse(_request, _result);
        }
    }
}
