using UniSpyServer.Servers.WebServer.Abstraction;

namespace UniSpyServer.Servers.WebServer.Module.Sake.Structure.Response
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