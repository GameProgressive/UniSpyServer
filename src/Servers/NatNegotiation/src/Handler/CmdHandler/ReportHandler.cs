using System;
using System.Linq;
using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Contract;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Redis;
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
    [HandlerContract(RequestType.Report)]
    public sealed class ReportHandler : CmdHandlerBase
    {
        private new ReportRequest _request => (ReportRequest)base._request;
        private new ReportResult _result { get => (ReportResult)base._result; set => base._result = value; }
        private NatInitInfo _userInfo;
        public ReportHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new ReportResult();
        }

        protected override void DataOperation()
        {
            _userInfo = _redisClient.Values.FirstOrDefault(
            k => k.ClientIndex == _request.ClientIndex
            && k.PortType == _request.PortType
            && k.Cookie == _request.Cookie);

            if (_userInfo == null)
            {
                // LogWriter.Info("No user found in redis.");
                return;
            }
            switch (_request.NatResult)
            {
                case NatNegResult.Success:
                    // if there is a success p2p connection, we delete the init info in redis
                    _redisClient.Values.Where(
                            k => k.PublicIPEndPoint == _client.Session.RemoteIPEndPoint
                            && k.Cookie == _request.Cookie).ToList()
                            .ForEach(k => _redisClient.DeleteKeyValue(k));
                    break;
                case NatNegResult.DeadBeatPartner:
                    LogWriter.Warning($"Parter of client {_client.Session.RemoteIPEndPoint} has no response.");
                    goto default;
                case NatNegResult.InitTimeOut:
                    LogWriter.Info($"Client {_client.Session.RemoteIPEndPoint} nat initialization failed.");
                    break;
                case NatNegResult.PingTimeOut:
                    LogWriter.Info($"Client {_client.Session.RemoteIPEndPoint} nat ping failed.");
                    goto default;
                case NatNegResult.UnknownError:
                    LogWriter.Info($"Client {_client.Session.RemoteIPEndPoint} nat negotiation unknown error occured.");
                    break;
                default:
                    foreach (NatPortType portType in Enum.GetValues(typeof(NatPortType)))
                    {
                        var request = new ConnectRequest
                        {
                            PortType = portType,
                            Version = _request.Version,
                            Cookie = _request.Cookie,
                            IsUsingRelay = true
                        };
                        new ConnectHandler(_client, request).Handle();
                    }

                    _userInfo.RetryNatNegotiationTime++;
                    _redisClient.SetValue(_userInfo);
                    break;
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new ReportResponse(_request, _result);
        }
    }
}
