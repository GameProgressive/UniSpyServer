using System.Threading.Tasks;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.QueryReport.Contract.Request;
using UniSpy.Server.ServerBrowser.Aggregate.Packet.Response;
using UniSpy.Server.ServerBrowser.Application;

namespace UniSpy.Server.ServerBrowser.Handler.CmdHandler.AdHoc
{
    public class AdHocHandler : CmdHandlerBase
    {
        public new HeartBeatRequest _request => (HeartBeatRequest)base._request;
        public AdHocHandler(IRequest request) : base(null, request)
        {
        }
        /// <summary>
        /// We do not need to parse the request, the request we get is already parsed by other qr server
        /// </summary>
        protected override void RequestCheck() { }
        protected override void ResponseConstruct()
        {
            // todo add response
            // _response = new AdHocResponse(_request);
        }
        protected override void Response()
        {
            var clients = ClientManager.GetClient(_request.GameName);
            Parallel.ForEach(clients, client =>
            {
                if (((Client)client).Info.GameName == _request.GameName)
                {
                    client.Send(_response);
                }
            });
        }
    }
}