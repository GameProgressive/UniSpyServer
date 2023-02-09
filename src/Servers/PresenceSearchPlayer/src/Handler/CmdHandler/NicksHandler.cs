using UniSpyServer.Servers.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Request;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Response;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.Servers.PresenceSearchPlayer.Application;

/////////////////////////Finished?/////////////////////////////////
namespace UniSpyServer.Servers.PresenceSearchPlayer.Handler.CmdHandler
{
    /// <summary>
    /// Uses a email and namespaceid to find all nick in this account
    /// </summary>

    public sealed class NicksHandler : CmdHandlerBase
    {
        private new NicksResult _result { get => (NicksResult)base._result; set => base._result = value; }
        private new NicksRequest _request => (NicksRequest)base._request;
        public NicksHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new NicksResult();
        }

        protected override void DataOperation()
        {
            try
            {
                _result.DataBaseResults = StorageOperation.Persistance.GetAllNickAndUniqueNick(_request.Email,
                                                                     _request.Password,
                                                                     _request.NamespaceID);

                //we store data in strong type so we can use in next step
            }
            catch (System.Exception e)
            {
                throw new GPDatabaseException($"Unknown error occurs in database operation.", e);
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new NicksResponse(_request, _result);
        }
    }
}
