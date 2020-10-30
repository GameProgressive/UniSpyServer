using GameSpyLib.Abstraction.Interface;
using GameSpyLib.Database.DatabaseModel.MySql;
using PresenceConnectionManager.Entity.Structure.Request.Buddy;
using PresenceSearchPlayer.Entity.Enumerator;
using System.Collections.Generic;
using System.Linq;
namespace PresenceConnectionManager.Handler.CommandHandler.Buddy
{
    /// <summary>
    /// handles dell buddy request,remove friends from friends list
    /// </summary>
    public class DelBuddyHandler : PCMCommandHandlerBase
    {
        //PCMSession _session;
        //Dictionary<string, string> _recv;
        protected DelBuddyRequest _request;
        //delete friend in database then send bm_revoke message to friend
        public DelBuddyHandler(ISession client, Dictionary<string, string> recv) : base(client, recv)
        {
            _request = new DelBuddyRequest(recv);
        }

        protected override void CheckRequest()
        {
            _errorCode = _request.Parse();
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
                    _errorCode = GPError.DatabaseError;
                    return;
                }
                db.Friends.Remove(result.FirstOrDefault());
            }
        }
    }
}
