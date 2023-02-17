using UniSpy.Server.WebServer.Abstraction;
using UniSpy.Server.WebServer.Module.Sake.Entity.Structure.Response;
using UniSpy.Server.WebServer.Module.Sake.Structure.Request;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.WebServer.Module.Sake.Handler
{
    
    internal class SearchForRecordsHandler : CmdHandlerBase
    {
        protected new SearchForRecordsRequest _request => (SearchForRecordsRequest)base._request;
        public SearchForRecordsHandler(IClient client, IRequest request) : base(client, request)
        {

        }

        protected override void ResponseConstruct()
        {
            _response = new SearchForRecordResponse(_request, null);
        }
    }
}
