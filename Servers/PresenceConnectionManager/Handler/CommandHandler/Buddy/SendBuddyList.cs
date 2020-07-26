using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Database.DatabaseModel.MySql;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Handler.Buddy.SendBuddies
{
    /// <summary>
    /// Send friendlist, friends message, friends add request to player when logged in.
    /// </summary>
    public class SendBuddyList : PCMCommandHandlerBase
    {


        public SendBuddyList(ISession client, Dictionary<string, string> recv) : base(client, recv)
        {

        }

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


        protected override void ConstructResponse()
        {
            base.ConstructResponse();
        }

        protected override void DataOperation()
        {
            if (_session.UserInfo.BuddiesSent)
            {
                return;
            }

            _session.UserInfo.BuddiesSent = true;

            using (var db = new retrospyContext())
            {
                var buddies = db.Friends.Where(
                    f => f.Profileid == _session.UserInfo.ProfileID
                && f.Namespaceid == _session.UserInfo.NamespaceID);
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
                    {
                        _sendingBuffer += @",";
                    }
                }
                _sendingBuffer += @"\final\";
            }
        }
    }
}
