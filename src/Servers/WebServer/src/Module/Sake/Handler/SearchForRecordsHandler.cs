using UniSpy.Server.WebServer.Abstraction;
using UniSpy.Server.WebServer.Module.Sake.Contract.Response;
using UniSpy.Server.WebServer.Module.Sake.Contract.Request;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.WebServer.Application;

namespace UniSpy.Server.WebServer.Module.Sake.Handler
{

    internal class SearchForRecordsHandler : CmdHandlerBase
    {
        protected new SearchForRecordsRequest _request => (SearchForRecordsRequest)base._request;
        public SearchForRecordsHandler(Client client, SearchForRecordsRequest request) : base(client, request)
        {

        }

        protected override void ResponseConstruct()
        {
            _response = new SearchForRecordResponse(_request, null);
        }
    }
}
