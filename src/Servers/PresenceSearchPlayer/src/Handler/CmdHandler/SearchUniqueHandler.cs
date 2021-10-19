using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Contract;
using PresenceSearchPlayer.Entity.Exception.General;
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
    /// Search with uniquenick and namespace
    /// </summary>
    [HandlerContract("searchunique")]
    public sealed class SearchUniqueHandler : CmdHandlerBase
    {
        private new SearchUniqueRequest _request => (SearchUniqueRequest)base._request;
        private new SearchUniqueResult _result
        {
            get => (SearchUniqueResult)base._result;
            set => base._result = value;
        }
        public SearchUniqueHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new SearchUniqueResult();
        }
        protected override void DataOperation()
        {
            try
            {
                using (var db = new unispyContext())
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
