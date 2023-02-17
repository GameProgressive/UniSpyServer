using UniSpy.Server.NatNegotiation.Entity.Enumerate;
using UniSpy.Server.Core.Abstraction.BaseClass;

namespace UniSpy.Server.NatNegotiation.Application
{
    public sealed class ClientInfo : ClientInfoBase
    {
        public uint? Cookie { get; set; }
        public NatClientIndex? ClientIndex { get; set; }
        public bool IsNeigotiating { get; set; }
        public ClientInfo() : base()
        {
        }
    }
}