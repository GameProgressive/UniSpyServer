using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Structure.Request;
using System.Collections.Generic;
using System.Linq;
using PresenceSearchPlayer.Entity.Structure.Result;
using PresenceSearchPlayer.Entity.Structure.Response;

namespace PresenceSearchPlayer.Handler.CmdHandler
{
    /// <summary>
    /// Search with uniquenick and namespace
    /// </summary>
    public class SearchUniqueHandler : PSPCmdHandlerBase
    {
        protected new SearchUniqueRequest _request => (SearchUniqueRequest)base._request;
        protected new SearchUniqueResult _result
        {
            get { return (SearchUniqueResult)base._result; }
            set { base._result = value; }
        }
        public SearchUniqueHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
        protected override void RequestCheck()
        {
            _result = new SearchUniqueResult();
        }
        protected override void DataOperation()
        {
            using (var db = new retrospyContext())
            {
                foreach (var id in _request.Namespaces)
                {
                    var result = from p in db.Profiles
                                 join n in db.Subprofiles on p.Profileid equals n.Profileid
                                 join u in db.Users on p.Userid equals u.Userid
                                 where n.Uniquenick == _request.Uniquenick
                                 && n.Namespaceid == id
                                 select new SearchUniqueDatabaseModel
                                 {
                                     Profileid = n.Profileid,
                                     Nick = p.Nick,
                                     Uniquenick = n.Uniquenick,
                                     Email = u.Email,
                                     Firstname = p.Firstname,
                                     Lastname = p.Lastname,
                                     NamespaceID = n.Namespaceid
                                 };
                    _result.DatabaseResults.AddRange(result.ToList());
                }
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new SearchUniqueResponse(_request,_result);
        }
    }
}
