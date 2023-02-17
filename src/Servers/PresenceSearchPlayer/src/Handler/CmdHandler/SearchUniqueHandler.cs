using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpy.Server.PresenceSearchPlayer.Exception.General;
using UniSpy.Server.PresenceSearchPlayer.Contract.Request;
using UniSpy.Server.PresenceSearchPlayer.Contract.Response;
using UniSpy.Server.PresenceSearchPlayer.Contract.Result;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.PresenceSearchPlayer.Application;

namespace UniSpy.Server.PresenceSearchPlayer.Handler.CmdHandler
{
    /// <summary>
    /// Search with uniquenick and namespace
    /// </summary>

    public sealed class SearchUniqueHandler : CmdHandlerBase
    {
        private new SearchUniqueRequest _request => (SearchUniqueRequest)base._request;
        private new SearchUniqueResult _result { get => (SearchUniqueResult)base._result; set => base._result = value; }
        public SearchUniqueHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new SearchUniqueResult();
        }
        protected override void DataOperation()
        {
            try
            {
                _result.DatabaseResults = StorageOperation.Persistance.GetMatchedInfosByNamespaceId(_request.NamespaceIds, _request.Uniquenick);
            }
            catch (System.Exception e)
            {
                throw new GPDatabaseException("Unknown error occurs in database operation.", e);
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new SearchUniqueResponse(_request, _result);
        }
    }
}
