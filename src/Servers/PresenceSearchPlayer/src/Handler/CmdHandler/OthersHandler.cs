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

    /// <summary>
    /// Get buddy's information
    /// </summary>
    [HandlerContract("others")]
    public sealed class OthersHandler : CmdHandlerBase
    {
        private new OthersRequest _request => (OthersRequest)base._request;

        private new OthersResult _result{ get => (OthersResult)base._result; set => base._result = value; }
        public OthersHandler(ISession session, IRequest request) : base(session, request)
        {
            _result = new OthersResult();
        }

        protected override void DataOperation()
        {
            try
            {
                using (var db = new UniSpyContext())
                {
                    var result = from b in db.Friends
                                 where b.ProfileId == _request.ProfileId && b.Namespaceid == _request.NamespaceID
                                 select b.Targetid;

                    foreach (var info in result)
                    {
                        var b = from p in db.Profiles
                                join n in db.Subprofiles on p.ProfileId equals n.ProfileId
                                join u in db.Users on p.Userid equals u.Userid
                                where n.Namespaceid == _request.NamespaceID
                                && n.ProfileId == info && n.Gamename == _request.GameName
                                select new OthersDatabaseModel
                                {
                                    ProfileId = p.ProfileId,
                                    Nick = p.Nick,
                                    Uniquenick = n.Uniquenick,
                                    Lastname = p.Lastname,
                                    Firstname = p.Firstname,
                                    Userid = u.Userid,
                                    Email = u.Email
                                };

                        _result.DatabaseResults.Add(b.First());
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
            _response = new OthersResponse(_request, _result);
        }
    }
}
