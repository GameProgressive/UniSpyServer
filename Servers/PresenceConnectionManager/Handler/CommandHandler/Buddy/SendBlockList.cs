using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Database.DatabaseModel.MySql;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Handler.CommandHandler.Buddy
{
    public class SendBlockList : PCMCommandHandlerBase
    {
        List<Blocked> _blockedList;
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
                var result = db.Blocked.Where(
                    f => f.Profileid == _session.UserData.ProfileID
                && f.Namespaceid == _session.UserData.NamespaceID);

                _blockedList = result.ToList();

            }
        }


        protected override void BuildNormalResponse()
        {
            _sendingBuffer = $@"\blk\{_blockedList.Count()}\list\";
            foreach (var user in _blockedList)
            {
                _sendingBuffer += user.Profileid;

                if (user != _blockedList.Last())
                {
                    _sendingBuffer += @",";
                }
            }
            _sendingBuffer += @"\final\";
        }
    }
}

