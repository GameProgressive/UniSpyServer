using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Enumerate;
using System.Net;
using UniSpyLib.Extensions;

namespace NATNegotiation.Entity.Structure.Request
{
    internal sealed class InitRequest : InitRequestBase
    {
        public InitRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
