
using UniSpy.Server.Master.Abstraction.BaseClass;
using UniSpy.Server.Master.Contract.Result;

namespace UniSpy.Server.Master.Contract.Response
{
    public sealed class EchoResponse : ResponseBase
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