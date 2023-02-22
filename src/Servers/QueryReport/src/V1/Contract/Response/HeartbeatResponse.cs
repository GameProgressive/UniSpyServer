using UniSpy.Server.QueryReport.V1.Abstraction.BaseClass;
using UniSpy.Server.QueryReport.V1.Contract.Request;
using UniSpy.Server.QueryReport.V1.Contract.Result;

namespace UniSpy.Server.QueryReport.V1.Contract.Response
{
    public class HeartbeatResponse : ResponseBase
    {
        private new HeartbeatResult _result => (HeartbeatResult)base._result;
        public HeartbeatResponse(HeartbeatRequest request, HeartbeatResult result) : base(request, result)
        {
        }
        public override void Build()
        {
            SendingBuffer = $@"/secure/{_result.Challenge}/final/";
        }
    }
}