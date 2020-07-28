using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Database.DatabaseModel.MySql;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Handler.Buddy
{
    public class SendBlockList : PCMCommandHandlerBase
    {
        public SendBlockList(ISession client, Dictionary<string, string> recv) : base(client, recv)
        {
        }

        protected override void DataOperation()
        {
            if (_session.UserData.BlockListSent)
            {
                return;
            }

            _session.UserData.BlockListSent = true;

            using (var db = new retrospyContext())
            {
                var buddies = db.Blocked.Where(
                    f => f.Profileid == _session.UserData.ProfileID
                && f.Namespaceid == _session.UserData.NamespaceID);
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
                    {
                        _sendingBuffer += @",";
                    }
                }
                _sendingBuffer += @"\final\";
            }
        }
    }
}
