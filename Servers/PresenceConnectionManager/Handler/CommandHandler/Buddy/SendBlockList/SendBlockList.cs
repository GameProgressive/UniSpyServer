using GameSpyLib.Database.DatabaseModel.MySql;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Handler.Buddy.SendBlockList
{
    public class SendBlockList : GPCMHandlerBase
    {
        protected SendBlockList(GPCMSession session, Dictionary<string, string> recv) : base(session, recv)
        {
        }

        protected override void DataBaseOperation(GPCMSession session, Dictionary<string, string> recv)
        {
            if (session.UserInfo.BlockListSent)
                return;
            session.UserInfo.BlockListSent = true;
            using (var db = new retrospyContext())
            {
                var buddies = db.Blocked.Where(
                    f => f.Profileid == session.UserInfo.Profileid
                && f.Namespaceid == session.UserInfo.NamespaceID);
                //if (buddies.Count() == 0)
                //{
                //    _sendingBuffer = @"\blk\0\list\\final\";
                //    return;
                //}
                _sendingBuffer = @"\blk\" + buddies.Count() + @"\list\";
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
