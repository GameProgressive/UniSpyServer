using UniSpy.Server.Master.Abstraction.BaseClass;
using UniSpy.Server.Master.Contract.Request;
using UniSpy.Server.Master.Contract.Result;

namespace UniSpy.Server.Master.Contract.Response
{
    public sealed class HeartbeatResponse : ResponseBase
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