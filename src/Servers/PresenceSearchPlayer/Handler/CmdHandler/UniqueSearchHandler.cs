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
    internal class UniqueSearchHandler : PSPCmdHandlerBase
    {
        protected new UniqueSearchRequest _request
        {
            get { return (UniqueSearchRequest)base._request; }
        }
        protected new UniqueSearchResult _result
        {
            get { return (UniqueSearchResult)base._result; }
            set { base._result = value; }
        }
        public UniqueSearchHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new UniqueSearchResult();
        }
        protected override void DataOperation()
        {
            using (var db = new unispyContext())
            {
                var result = from p in db.Profiles
                             join n in db.Subprofiles on p.Profileid equals n.Profileid
                             where n.Uniquenick == _request.PreferredNick
                             && n.Namespaceid == _request.NamespaceID
                             && n.Gamename == _request.GameName
                             select p.Profileid;

                if (result.Count() != 0)
                {
                    _result.IsUniquenickExist = true;
                }
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new UniqueSearchResponse(_request, _result);
        }
    }
}
