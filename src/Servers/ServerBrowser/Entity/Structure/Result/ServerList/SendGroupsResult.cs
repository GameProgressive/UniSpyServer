using QueryReport.Entity.Structure.Group;
using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Enumerate;

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
