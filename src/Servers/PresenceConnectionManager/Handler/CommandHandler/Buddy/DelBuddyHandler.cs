using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;
using PresenceConnectionManager.Entity.Structure.Request.Buddy;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;
using System.Linq;
using PresenceConnectionManager.Abstraction.BaseClass;

namespace PresenceConnectionManager.Handler.CommandHandler
{
    /// <summary>
    /// handles dell buddy request,remove friends from friends list
    /// </summary>
    public class DelBuddyHandler : PCMCommandHandlerBase
    {
        //PCMSession _session;
        //Dictionary<string, string> _recv;
        protected new DelBuddyRequest _request;
        //delete friend in database then send bm_revoke message to friend
        public DelBuddyHandler(ISession session, IRequest request) : base(session, request)
        {
            _request = (DelBuddyRequest)request;
        }

        protected override void DataOperation()
        {
            using (var db = new retrospyContext())
            {
                var result = from f in db.Friends
                             where f.Profileid == _request.DeleteProfileID && f.Namespaceid == _session.UserData.NamespaceID
                             select f;
                if (result.Count() != 1)
                {
                    _errorCode = GPErrorCode.DatabaseError;
                    return;
                }
                db.Friends.Remove(result.FirstOrDefault());
            }
        }
    }
}
