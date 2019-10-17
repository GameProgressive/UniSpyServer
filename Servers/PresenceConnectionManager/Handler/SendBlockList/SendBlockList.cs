using System;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Handler.SendBlockList
{
    public class SendBlockList
    {
        public static void Send(GPCMSession session)
        {
            if (session.PlayerInfo.BlockListSent)
                return;
            session.PlayerInfo.BlockListSent = true;

            string sendingBuffer= @"\blk\";
            Dictionary<string, object> result = SendBlockListQuery.SearchBlockList(session.PlayerInfo.Profileid, session.PlayerInfo.Namespaceid);
            if (result!=null)
            {
                sendingBuffer += result.Count +@"\list\";
                foreach (KeyValuePair<string, object> id in result)
                {
                    sendingBuffer += Convert.ToUInt32(id.Value) + @",";
                    if (id.Equals(result.Last()))
                    {
                        sendingBuffer += Convert.ToUInt32(id.Value);
                    }
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
