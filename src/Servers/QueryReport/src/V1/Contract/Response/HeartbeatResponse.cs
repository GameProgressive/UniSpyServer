using UniSpy.Server.QueryReport.V1.Abstraction.BaseClass;
using UniSpy.Server.QueryReport.V1.Contract.Request;

namespace UniSpy.Server.QueryReport.V1.Contract.Response
{
    public sealed class HeartbeatResponse : ResponseBase
    {
        public const string Challenge = "000000";
        public HeartbeatResponse(HeartbeatRequest request) : base(request, null)
        {
        }
        public override void Build()
        {
            SendingBuffer = $@"/secure/{Challenge}/final/";
        }
    }
}