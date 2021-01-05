using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Structure.Request;
using System.Collections.Generic;
using System.Linq;
using PresenceSearchPlayer.Entity.Structure.Result;

namespace PresenceSearchPlayer.Handler.CmdHandler
{
    public class UniqueSearchHandler : PSPCmdHandlerBase
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
        }

        protected override void DataOperation()
        {
            _result = new UniqueSearchResult();
            using (var db = new retrospyContext())
            {
                var result = from p in db.Profiles
                             join n in db.Subprofiles on p.Profileid equals n.Profileid
                             where n.Uniquenick == _request.PreferredNick
                             && n.Namespaceid == _request.NamespaceID
                             //&& n.Gamename == _request.GameName
                             select p.Profileid;

                if (result.Count() == 0)
                {
                    _result.IsUniquenickExist = false;
                }
            }
        }
    }
}
