using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;

namespace UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass
{
    public class ResultBase : UniSpyLib.Abstraction.BaseClass.ResultBase
    {
        public ResponseType? PacketType { get; set; }
        public ResultBase()
        {
        }
    }
}
