using GameSpyLib.Common.Entity.Interface;
using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.Buddy.BuddyStatusInfo
{
    public class BuddyStatusInfo : PCMCommandHandlerBase
    {

        public BuddyStatusInfo(IClient client, Dictionary<string, string> recv) : base(client, recv)
        {
        }
        //public static void SendBuddyStatusInfo(GPCMSession _session, uint[] profileids)
        //{
        //    Dictionary<string, object> result;

        //    foreach (uint profileid in profileids)
        //    {
        //        string sendingBuffer = @"\bm\" + (uint)GPBasic.BmStatus + @"\f\";
        //        result = SendBuddiesQuery.GetStatusInfo(profileid, _session.PlayerInfo.NamespaceID);
        //        sendingBuffer += profileid + @"\msg\";
        //        sendingBuffer += @"|s|" + Convert.ToUInt32(result["status"]);
        //        sendingBuffer += @"|ss|" + result["statstring"].ToString();
        //        sendingBuffer += @"|ls|" + result["location"].ToString();
        //        sendingBuffer += @"|ip|" + result["lastip"];
        //        sendingBuffer += @"|p|" + Convert.ToUInt32(result["port"]);
        //        sendingBuffer += @"|qm|" + result["quietflags"] + @"\final\";

        //        _session.SendAsync(sendingBuffer);
        //    }
    }
}
