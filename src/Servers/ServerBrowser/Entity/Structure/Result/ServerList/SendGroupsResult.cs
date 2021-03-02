using QueryReport.Entity.Structure.Redis;
using ServerBrowser.Abstraction.BaseClass;

namespace ServerBrowser.Entity.Structure.Result
{
    internal sealed class SendGroupsResult : ServerListResultBase
    {
        public PeerGroupInfo PeerGroupInfo { get; set; }
        public SendGroupsResult()
        {
        }
    }
}
