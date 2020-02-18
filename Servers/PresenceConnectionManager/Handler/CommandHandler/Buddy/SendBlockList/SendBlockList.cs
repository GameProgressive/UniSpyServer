using GameSpyLib.Database.DatabaseModel.MySql;
using System.Linq;

namespace PresenceConnectionManager.Handler.Buddy.SendBlockList
{
    public class SendBlockList : GPCMHandlerBase
    {

        protected override void DataBaseOperation(GPCMSession session)
        {
            if (session.UserInfo.BlockListSent)
                return;
            session.UserInfo.BlockListSent = true;
            using (var db = new RetrospyDB())
            {
                var buddies = db.Blockeds.Where(
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
