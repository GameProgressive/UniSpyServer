using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Structure.Request;
using PresenceSearchPlayer.Entity.Structure.Response;
using PresenceSearchPlayer.Entity.Structure.Result;
using System.Collections.Generic;
using System.Linq;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;

namespace PresenceSearchPlayer.Handler.CmdHandler
{

    /// <summary>
    /// Get buddy's information
    /// </summary>
    internal class OthersHandler : PSPCmdHandlerBase
    {
        protected new OthersRequest _request
        {
            get { return (OthersRequest)base._request; }
        }
        protected new OthersResult _result
        {
            get { return (OthersResult)base._result; }
            set { base._result = value; }
        }
        public OthersHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void RequestCheck()
        {
            _result = new OthersResult();
        }

        protected override void DataOperation()
        {
            using (var db = new retrospyContext())
            {
                var result = from b in db.Friends
                             where b.Profileid == _request.ProfileID && b.Namespaceid == _request.NamespaceID
                             select b.Targetid;

                foreach (var info in result)
                {
                    var b = from p in db.Profiles
                            join n in db.Subprofiles on p.Profileid equals n.Profileid
                            join u in db.Users on p.Userid equals u.Userid
                            where n.Namespaceid == _request.NamespaceID
                            && n.Profileid == info && n.Gamename == _request.GameName
                            select new OthersDatabaseModel
                            {
                                Profileid = p.Profileid,
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

        protected override void ResponseConstruct()
        {
            _response = new OthersResponse(_request, _result);
        }
    }
}
