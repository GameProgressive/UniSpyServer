using GameSpyLib.Database.DatabaseModel.MySql;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Handler.Buddy.SendBuddies
{
    /// <summary>
    /// Send friendlist, friends message, friends add request to player when he logged in.
    /// </summary>
    public class SendBuddyList : GPCMHandlerBase
    {
        //**********************************************************
        //\bm\<MESSAGE>\f\<from profileid>\msg\<>\final\
        //\bm\<UTM>\f\<from profileid>\msg\<>\final\
        //\bm\<REQUEST>\f\<from profileid>\msg\|signed|\final\
        //\bm\<AUTH>\f\<from profileid>\final\
        //\bm\<REVOKE>\f\<from profileid>\final\
        //\bm\<STATUS>\f\<from profileid>\msg\|s|<status code>|ss|<status string>|ls|<location string>|ip|<>|p|<port>|qm|<quiet mode falgs>\final\
        //\bm\<INVITE>\f\<from profileid>\msg\|p|<productid>|l|<location string>
        //\bm\<ping>\f\<from profileid>\msg\final\
        //\bm\<pong>\f\<from profileid>\final\
        protected SendBuddyList(GPCMSession session, Dictionary<string, string> recv) : base(session, recv)
        {
        }

        protected override void ConstructResponse(GPCMSession session, Dictionary<string, string> recv)
        {
            base.ConstructResponse(session, recv);
        }

        protected override void DataBaseOperation(GPCMSession session, Dictionary<string, string> recv)
        {
            if (session.UserInfo.BuddiesSent)
                return;
            session.UserInfo.BuddiesSent = true;
            //return;
            using (var db = new RetrospyDB())
            {
                var buddies = db.Friends.Where(
                    f => f.Profileid == session.UserInfo.Profileid
                && f.Namespaceid == session.UserInfo.NamespaceID);
                //if (buddies.Count() == 0)
                //{
                //    _sendingBuffer = @"\bdy\0\list\\final\";
                //    return;
                //}
                _sendingBuffer = @"\bdy\" + buddies.Count() + @"\list\";
                foreach (var b in buddies)
                {

                    _sendingBuffer += b.Profileid;
                    if (b != buddies.Last())
                        _sendingBuffer += @",";
                }
                _sendingBuffer += @"\final\";
            }
        }
    }
}
