using UniSpy.Server.ServerBrowser.V1.Abstraction.BaseClass;
using UniSpy.Server.ServerBrowser.V1.Application;
using UniSpy.Server.ServerBrowser.V1.Contract.Request;
using UniSpy.Server.ServerBrowser.V1.Contract.Response;
using UniSpy.Server.ServerBrowser.V1.Contract.Result;

namespace UniSpy.Server.ServerBrowser.V1.Handler.CmdHandler
{
    public class ListHandler : CmdHandlerBase
    {
        private new ListResult _result { get => (ListResult)base._result; set => base._result = value; }
        private new ListResponse _response { get => (ListResponse)base._response; set => base._response = value; }
        private new ListRequest _request => (ListRequest)base._request;
        public ListHandler(Client client, RequestBase request) : base(client, request)
        {
            _result = new ListResult();
        }
        protected override void RequestCheck()
        {
            switch (_request.Type)
            {

                case ListRequestType.Info:
                case ListRequestType.Basic:
                    _result.ServersInfo = QueryReport.V1.Application.StorageOperation.Persistance.GetServersInfo(_request.GameName);
                    break;
                case ListRequestType.Group:
                    _result.PeerRoomsInfo =
                    QueryReport.V2.Application.StorageOperation.Persistance.GetPeerRoomsInfo(_request.GameName);
                    // todo
                    break;
            }
        }
        protected override void ResponseConstruct()
        {
            _response = new ListResponse(_request, _result);
        }
    }
}