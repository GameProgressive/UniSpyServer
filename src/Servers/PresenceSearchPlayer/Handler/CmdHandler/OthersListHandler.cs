using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Structure.Request;
using System.Linq;
using PresenceSearchPlayer.Entity.Structure.Result;
using PresenceSearchPlayer.Entity.Structure.Response;

namespace PresenceSearchPlayer.Handler.CmdHandler
{

    public class OthersListHandler : PSPCmdHandlerBase
    {
        protected new OthersListRequest _request
        {
            get { return (OthersListRequest)base._request; }
        }
        protected new OthersListResult _result
        {
            get { return (OthersListResult)base._result; }
            set { base._result = value; }
        }

        public OthersListHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        //request: \otherslist\sesskey\<searcher's sesskey>\profileid\<searcher's pid>\numopids\<how many pid in his list>
        //\opids\|<opid1>|<opid2>|******\namespaceid\<>\gamename\<>\final\
        protected override void RequestCheck()
        {
            _result = new OthersListResult(_request);
        }
        protected override void DataOperation()
        {
            using (var db = new retrospyContext())
            {
                foreach (var pid in _request.ProfileIDs)
                {
                    var result = from n in db.Subprofiles
                                 where n.Profileid == pid
                                 && n.Namespaceid == _request.NamespaceID
                                 //select new { uniquenick = n.Uniquenick };
                                 select new OthersListDatabaseModel
                                 {
                                     ProfileID = n.Profileid,
                                     Uniquenick = n.Uniquenick
                                 };

                    _result.DatabaseResults.AddRange(result.ToList());
                }
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new OthersListResponse(_result);
        }
    }
}
