using UniSpy.Server.NatNegotiation.Entity.Enumerate;

namespace UniSpy.Server.NatNegotiation.Abstraction.BaseClass
{
    public class ResultBase : UniSpy.Server.Core.Abstraction.BaseClass.ResultBase
    {
        public ResponseType? PacketType { get; set; }
        public ResultBase()
        {
        }
    }
}
