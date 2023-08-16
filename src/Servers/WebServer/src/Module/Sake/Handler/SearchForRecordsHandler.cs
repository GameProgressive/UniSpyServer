using UniSpy.Server.WebServer.Abstraction;
using UniSpy.Server.WebServer.Module.Sake.Contract.Response;
using UniSpy.Server.WebServer.Module.Sake.Contract.Request;
using UniSpy.Server.WebServer.Application;
using UniSpy.Server.WebServer.Module.Sake.Contract.Result;

namespace UniSpy.Server.WebServer.Module.Sake.Handler
{

    internal class SearchForRecordsHandler : CmdHandlerBase
    {
        private new SearchForRecordsRequest _request => (SearchForRecordsRequest)base._request;
        private new SearchForRecordsResult _result { get => (SearchForRecordsResult)base._result; set => base._result = value; }
        public SearchForRecordsHandler(Client client, SearchForRecordsRequest request) : base(client, request)
        {
            _result = new SearchForRecordsResult();
        }
        protected override void DataOperation()
        {
            base.DataOperation();
            // search user data from database
        }
        protected override void ResponseConstruct()
        {
            _response = new SearchForRecordResponse(_request, _result);
        }
    }
}
