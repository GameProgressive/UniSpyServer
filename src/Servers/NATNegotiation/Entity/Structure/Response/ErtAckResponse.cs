
using System.Net;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Entity.Structure.Request;
using UniSpyLib.Abstraction.BaseClass;

namespace NATNegotiation.Entity.Structure.Response
{
    public class ErtAckResponse : InitResponse
    {
        public ErtAckResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
    }
}
