using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Handler.Buddy.SendBlockList
{
    public class SendBlockListHandler
    {
        public static void SendBlockList(GPCMSession session)
        {
            if (session.PlayerInfo.BlockListSent)
                return;
            session.PlayerInfo.BlockListSent = true;

            string sendingBuffer= @"\blk\";
            Dictionary<string, object> result = SendBlockListQuery.SearchBlockList(session.PlayerInfo.Profileid, session.PlayerInfo.Namespaceid);
            uint[] profileids;
            if (result!=null)
            {
                profileids = result.Values.Cast<uint>().ToArray();
                sendingBuffer += result.Count +@"\list\";
                for (int i = 0; i < profileids.Length; i++)
                {
                    //last one we do not add ,
                    if (i == profileids.Length - 1)
                    {
                        sendingBuffer += profileids[i];
                        break;
                    }
                    sendingBuffer += profileids[i] + @",";
                }
                sendingBuffer += @"\final\";
            }
            else
            {
                sendingBuffer = @"\blk\0\list\\final\";
            }

            session.SendAsync(sendingBuffer);
        }
    }
}
