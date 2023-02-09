using UniSpyServer.Servers.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Request;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Response;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.Servers.PresenceSearchPlayer.Application;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Handler.CmdHandler
{

    public sealed class UniqueSearchHandler : CmdHandlerBase
    {
        private new UniqueSearchRequest _request => (UniqueSearchRequest)base._request;
        private new UniqueSearchResult _result { get => (UniqueSearchResult)base._result; set => base._result = value; }
        public UniqueSearchHandler(IClient client, IRequest request) : base(client, request)
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
