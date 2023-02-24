using UniSpy.Server.NatNegotiation.Abstraction.BaseClass;

namespace UniSpy.Server.NatNegotiation.Contract.Response
{
    public sealed class InitResponse : CommonResponseBase
    {
        public InitResponse(RequestBase request, ResultBase result) : base(request, result)
        {
        }
    }
}
