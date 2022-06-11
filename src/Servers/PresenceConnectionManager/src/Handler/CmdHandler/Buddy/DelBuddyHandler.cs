using System.Linq;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;

namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.CmdHandler.Buddy
{
    /// <summary>
    /// handles dell buddy request,remove friends from friends list
    /// </summary>

    public sealed class DelBuddyHandler : Abstraction.BaseClass.CmdHandlerBase
    {
        private new DelBuddyRequest _request => (DelBuddyRequest)base._request;
        //delete friend in database then send bm_revoke message to friend
        public DelBuddyHandler(IClient client, IRequest request) : base(client, request)
        {
        }

        protected override void DataOperation()
        {
            using (var db = new UniSpyContext())
            {
                var result = from friend in db.Friends
                             where friend.ProfileId == _request.DeleteProfileID
                                   && friend.Namespaceid == _client.Info.SubProfileInfo.NamespaceId
                             select friend;
                if (result.Count() == 0)
                {
                    throw new GPDatabaseException("No buddy found in database.");
                }
                else if (result.Count() > 1)
                {
                    throw new GPDatabaseException("More than one buddy found in database, please check database.");
                }

                db.Friends.Remove(result.First());
            }
        }
    }
}
