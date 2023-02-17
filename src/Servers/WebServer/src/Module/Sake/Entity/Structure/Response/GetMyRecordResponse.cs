using UniSpy.Server.WebServer.Abstraction;

namespace UniSpy.Server.WebServer.Module.Sake.Structure.Response
{
    public class GetMyRecordResponse : ResponseBase
    {
        public GetMyRecordResponse(RequestBase request, ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            throw new System.NotImplementedException();
        }
    }
}