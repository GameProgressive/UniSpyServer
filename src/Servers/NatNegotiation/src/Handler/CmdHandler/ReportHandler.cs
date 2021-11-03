using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Application;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.Servers.NatNegotiation.Entity.Exception;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Redis;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Response;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Result;
using System;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.Servers.NatNegotiation.Entity.Contract;

namespace UniSpyServer.Servers.NatNegotiation.Handler.CmdHandler
{
    /// <summary>
    /// Get nat neg result report success or fail
    /// </summary>
    [HandlerContract(RequestType.Report)]
    public sealed class ReportHandler : CmdHandlerBase
    {
        private new ReportRequest _request => (ReportRequest)base._request;
        private new ReportResult _result
        {
            get => (ReportResult)base._result;
            set => base._result = value;
        }
        private UserInfo _userInfo;
        public ReportHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new ReportResult();
        }

        protected override void DataOperation()
        {
            //_userInfo.IsGotReportPacket = true;
            var fullKey = new UserInfoRedisKey()
            {
                ServerID = ServerFactory.Server.ServerID,
                RemoteIPEndPoint = _session.RemoteIPEndPoint,
                PortType = _request.PortType,
                Cookie = _request.Cookie
            };

            try
            {
                _userInfo = UserInfoRedisOperator.GetSpecificValue(fullKey);
            }
            catch
            {
                throw new NNException("No user found in redis.");
            }

            if (_request.NatResult != NatNegResult.Success)
            {
                foreach (NatPortType portType in Enum.GetValues(typeof(NatPortType)))
                {
                    var request = new ConnectRequest
                    {
                        PortType = portType,
                        Version = _request.Version,
                        Cookie = _request.Cookie
                    };
                    new ConnectHandler(_session, request).Handle();
                }

                _userInfo.RetryNATNegotiationTime++;
                UserInfoRedisOperator.SetKeyValue(fullKey, _userInfo);
            }
            else
            {
                // natnegotiation successed we delete the negotiator
                UserInfoRedisOperator.DeleteKeyValue(fullKey);
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new ReportResponse(_request, _result);
        }
    }
}
