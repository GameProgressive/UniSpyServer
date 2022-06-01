using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure
{
    public class ClientInfo : ClientInfoBase
    {
        public uint? Cookie { get; set; }
        public NatClientIndex? ClientIndex { get; set; }
        public ClientInfo() : base()
        {
        }
    }
}