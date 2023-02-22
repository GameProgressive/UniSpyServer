
using UniSpy.Server.QueryReport.V1.Abstraction.BaseClass;

namespace UniSpy.Server.QueryReport.V1.Contract.Response
{
    public class EchoResponse : V1.Abstraction.BaseClass.ResponseBase
    {
        public EchoResponse(RequestBase request, ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            throw new System.NotImplementedException();
        }
    }
}