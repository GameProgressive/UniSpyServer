using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Database.DatabaseModel.MySql;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Handler.CommandHandler.Buddy
{
    /// <summary>
    /// Send friendlist, friends message, friends add request to player when logged in.
    /// </summary>
    public class SendBuddyList : PCMCommandHandlerBase
    {

        private List<Friends> _friendList;
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
            if (_session.UserData.BuddiesSent)
            {
                return;
            }

            _session.UserData.BuddiesSent = true;

            using (var db = new retrospyContext())
            {
                var result = db.Friends.Where(
                    f => f.Profileid == _session.UserData.ProfileID
                && f.Namespaceid == _session.UserData.NamespaceID);

                _friendList = result.ToList();
            }
        }


        protected override void BuildNormalResponse()
        {
            _sendingBuffer = @$"\bdy\{_friendList.Count()}\list\";
            foreach (var user in _friendList)
            {
                _sendingBuffer += user.Profileid;

                if (user != _friendList.Last())
                {
                    _sendingBuffer += @",";
                }
            }
            _sendingBuffer += @"\final\";
        }
    }
}
