using NatNegotiation.Abstraction.BaseClass;
using NatNegotiation.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;
namespace NatNegotiation.Entity.Structure.Request
{
    [Command((byte)5)]
    internal sealed class ConnectRequest : NNRequestBase
    {
        public NatPortType PortType { get; set; }
        public ConnectRequest() { }
    }
}