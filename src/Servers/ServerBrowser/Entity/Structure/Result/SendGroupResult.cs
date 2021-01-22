using QueryReport.Entity.Structure.Group;
using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Enumerate;

namespace ServerBrowser.Entity.Structure.Result
{
    internal sealed class SendGroupResult : UpdateOptionResultBase
    {
        public PeerGroupInfo PeerGroupInfo { get; set; }
        public SendGroupResult()
        {
            ResponseType = SBServerResponseType.
        }
    }
}
