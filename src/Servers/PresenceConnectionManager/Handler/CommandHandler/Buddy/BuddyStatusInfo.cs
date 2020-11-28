using UniSpyLib.Abstraction.Interface;
using System.Collections.Generic;
using PresenceConnectionManager.Abstraction.BaseClass;

namespace PresenceConnectionManager.Handler.CommandHandler
{
    public class BuddyStatusInfoHandler : PCMCommandHandlerBase
    {

        public BuddyStatusInfoHandler(ISession session, IRequest request) : base(session, request)
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
