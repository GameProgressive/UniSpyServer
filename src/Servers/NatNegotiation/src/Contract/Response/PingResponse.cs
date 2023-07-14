using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.NatNegotiation.Contract.Request;

namespace UniSpy.Server.NatNegotiation.Contract.Response
{
    public class PingResponse : ResponseBase
    {
        private new PingRequest _request => (PingRequest)base._request;
        public PingResponse(PingRequest request) : base(request, null)
        {
        }

        public override void Build()
        {
            SendingBuffer = _request.RawRequest;
        }
    }
}