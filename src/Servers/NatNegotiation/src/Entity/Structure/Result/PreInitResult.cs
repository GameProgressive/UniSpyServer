using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Result
{
    public sealed class PreInitResult : ResultBase
    {
        public int ClientIndex { get; private set; }
        public PreInitState State { get; private set; }
        public int ClientID { get; private set; }
        public PreInitResult()
        {
            PacketType = ResponseType.PreInitAck;
            State = PreInitState.Ready;
        }
    }
}