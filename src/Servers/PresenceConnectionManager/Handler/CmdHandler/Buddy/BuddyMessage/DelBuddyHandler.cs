using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Contract;
using PresenceConnectionManager.Entity.Structure.Request;
using PresenceSearchPlayer.Entity.Exception.General;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    /// <summary>
    /// handles dell buddy request,remove friends from friends list
    /// </summary>
    [HandlerContract("delbuddy")]
    internal sealed class DelBuddyHandler : PCMCmdHandlerBase
    {
        private new DelBuddyRequest _request => (DelBuddyRequest)base._request;
        //delete friend in database then send bm_revoke message to friend
        public DelBuddyHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            using (var db = new unispyContext())
            {
                var result = from friend in db.Friends
                             where friend.Profileid == _request.DeleteProfileID
                                   && friend.Namespaceid == _session.UserInfo.BasicInfo.NamespaceID
                             select friend;
                if (result.Count() == 0)
                {
                    throw new GPDatabaseException("No buddy found in database.");
                }
                else if (result.Count() > 1)
                {
                    throw new GPDatabaseException("More than one buddy found in database, please check database.");
                }

                db.Friends.Remove(result.FirstOrDefault());
            }
        }
    }
}
