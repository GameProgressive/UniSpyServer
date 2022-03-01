using UniSpyServer.Servers.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Contract;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Request;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Response;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Result;
using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Handler.CmdHandler
{

    [HandlerContract("otherslist")]
    public sealed class OthersListHandler : CmdHandlerBase
    {
        private new OthersListRequest _request => (OthersListRequest)base._request;

        private new OthersListResult _result{ get => (OthersListResult)base._result; set => base._result = value; }

        public OthersListHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new OthersListResult();
        }

        protected override void DataOperation()
        {
            try
            {
                using (var db = new UniSpyContext())
                {
                    foreach (var pid in _request.ProfileIDs)
                    {
                        var result = from n in db.Subprofiles
                                     where n.ProfileId == pid
                                     && n.NamespaceId == _request.NamespaceID
                                     //select new { uniquenick = n.Uniquenick };
                                     select new OthersListDatabaseModel
                                     {
                                         ProfileId = n.ProfileId,
                                         Uniquenick = n.Uniquenick
                                     };

                        _result.DatabaseResults.AddRange(result.ToList());
                    }
                }
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
