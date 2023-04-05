
using UniSpy.Server.QueryReport.V1.Abstraction.BaseClass;

namespace UniSpy.Server.QueryReport.V1.Contract.Response
{
    public sealed class EchoResponse : ResponseBase
    {
        public EchoResponse(RequestBase request) : base(request, null)
        {
        }

        public override void Build()
        {
            SendingBuffer = $@"/echo/{HeartbeatResponse.Challenge}/final/";
        }
    }
}