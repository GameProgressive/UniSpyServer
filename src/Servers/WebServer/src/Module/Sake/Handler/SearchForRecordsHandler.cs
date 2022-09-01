using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Module.Sake.Entity.Structure.Response;
using UniSpyServer.Servers.WebServer.Module.Sake.Structure.Request;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.WebServer.Module.Sake.Handler
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
