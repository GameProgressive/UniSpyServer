using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.Buddy.BuddyStatusInfo
{
    public class BuddyStatusInfo : CommandHandlerBase
    {
        protected BuddyStatusInfo(GPCMSession session, Dictionary<string, string> recv) : base(session, recv)
        {
        }
        //public static void SendBuddyStatusInfo(GPCMSession session, uint[] profileids)
        //{
        //    Dictionary<string, object> result;

        //    foreach (uint profileid in profileids)
        //    {
        //        string sendingBuffer = @"\bm\" + (uint)GPBasic.BmStatus + @"\f\";
        //        result = SendBuddiesQuery.GetStatusInfo(profileid, session.PlayerInfo.NamespaceID);
        //        sendingBuffer += profileid + @"\msg\";
        //        sendingBuffer += @"|s|" + Convert.ToUInt32(result["status"]);
        //        sendingBuffer += @"|ss|" + result["statstring"].ToString();
        //        sendingBuffer += @"|ls|" + result["location"].ToString();
        //        sendingBuffer += @"|ip|" + result["lastip"];
        //        sendingBuffer += @"|p|" + Convert.ToUInt32(result["port"]);
        //        sendingBuffer += @"|qm|" + result["quietflags"] + @"\final\";

        //        session.SendAsync(sendingBuffer);
        //    }
    }
}
