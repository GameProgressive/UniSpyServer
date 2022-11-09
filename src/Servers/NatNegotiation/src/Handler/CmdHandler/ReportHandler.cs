using System.Linq;
using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Response;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.NatNegotiation.Handler.CmdHandler
{
    /// <summary>
    /// Get nat neg result report success or fail
    /// </summary>

    public sealed class ReportHandler : CmdHandlerBase
    {
        private new ReportRequest _request => (ReportRequest)base._request;
        private new ReportResult _result { get => (ReportResult)base._result; set => base._result = value; }
        public ReportHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new ReportResult();
        }

        protected override void ResponseConstruct()
        {
            _response = new ReportResponse(_request, _result);
        }

        protected override void Response()
        {
            // we first response, the client will timeout if no response is received in some interval
            base.Response();
            LogWriter.Info($"[{_client.Connection.RemoteIPEndPoint}] natneg success: {(bool)_request.IsNatSuccess}, version: {_request.Version}, gamename: {_request.GameName}, nat type: {_request.NatType} mapping scheme: {_request.MappingScheme}, cookie: {_request.Cookie}, port type: {_request.PortType}");
            // when negotiation failed we log the information

            // if (!(bool)_request.IsNatSuccess)
            // {
            //     var request = new ConnectRequest
            //     {
            //         PortType = NatPortType.NN1,
            //         Version = _request.Version,
            //         Cookie = _request.Cookie,
            //         IsUsingRelay = true
            //     };
            //     new ConnectHandler(_client, request).Handle();
            //     var packets = _redisClient.Context.Where(k => k.Cookie == _request.Cookie).ToList();
            //     foreach (var packet in packets)
            //     {
            //         packet.RetryNatNegotiationTime++;
            //         _redisClient.SetValue(packet);
            //     }
            // }

            // switch (_request.IsNatSuccess)
            // {
            //     case NatNegResult.Success:
            //         // if there is a success p2p connection, we delete the init info in redis
            //         _redisClient.Context.Where(k => k.Cookie == _request.Cookie).ToList()
            //                 .ForEach(k => _redisClient.DeleteKeyValue(k));
            //         LogWriter.Info("Nat negotiation success.");
            //         break;
            //     case NatNegResult.DeadBeatPartner:
            //         LogWriter.Info($"Parter of client {_client.Connection.RemoteIPEndPoint} has no response.");
            //         goto default;
            //     case NatNegResult.InitTimeOut:
            //         LogWriter.Info($"Client {_client.Connection.RemoteIPEndPoint} nat initialization failed.");
            //         break;
            //     case NatNegResult.PingTimeOut:
            //         LogWriter.Info($"Client {_client.Connection.RemoteIPEndPoint} nat ping failed.");
            //         goto default;
            //     case NatNegResult.UnknownError:
            //         LogWriter.Info($"Client {_client.Connection.RemoteIPEndPoint} nat negotiation unknown error occured.");
            //         break;
            //     default:
            //         var request = new ConnectRequest
            //         {
            //             PortType = NatPortType.NN1,
            //             Version = _request.Version,
            //             Cookie = _request.Cookie,
            //             IsUsingRelay = true
            //         };
            //         new ConnectHandler(_client, request).Handle();
            //         var packets = _redisClient.Context.Where(k => k.Cookie == _request.Cookie).ToList();
            //         foreach (var packet in packets)
            //         {
            //             packet.RetryNatNegotiationTime++;
            //             _redisClient.SetValue(packet);
            //         }
            //         break;
            // }
        }
    }
}
