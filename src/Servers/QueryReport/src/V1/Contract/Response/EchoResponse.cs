
using UniSpy.Server.QueryReport.V1.Abstraction.BaseClass;
using UniSpy.Server.QueryReport.V1.Contract.Result;

namespace UniSpy.Server.QueryReport.V1.Contract.Response
{
    public sealed class EchoResponse : V1.Abstraction.BaseClass.ResponseBase
    {
        private new EchoResult _result => (EchoResult)base._result;
        public EchoResponse(RequestBase request, ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = $@"/echo/{_result.Challenge}/final/";
        }
    }
}