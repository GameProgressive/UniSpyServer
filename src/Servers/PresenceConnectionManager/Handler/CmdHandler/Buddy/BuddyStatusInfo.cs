using PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    internal class BuddyStatusInfoHandler : PCMCmdHandlerBase
    {

        public BuddyStatusInfoHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void ResponseConstruct()
        {
            throw new System.NotImplementedException();
        }

        protected override void DataOperation()
        {
            throw new System.NotImplementedException();
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
