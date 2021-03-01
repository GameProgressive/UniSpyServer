using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Application;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Entity.Structure;
using NATNegotiation.Entity.Structure.Misc;
using NATNegotiation.Entity.Structure.Request;
using NATNegotiation.Entity.Structure.Response;
using NATNegotiation.Entity.Structure.Result;
using NATNegotiation.Handler.SystemHandler.Manager;
using NATNegotiation.Handler.SystemHandler.Redis;
using System;
using UniSpyLib.Abstraction.Interface;

namespace NATNegotiation.Handler.CmdHandler
{
    /// <summary>
    /// Get nat neg result report success or fail
    /// </summary>
    internal sealed class ReportHandler : NNCmdHandlerBase
    {
        private new ReportRequest _request => (ReportRequest)base._request;
        private new ReportResult _result
        {
            get { return (ReportResult)base._result; }
            set { base._result = value; }
        }
        private NatUserInfo _userInfo;
        public ReportHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new ReportResult();
        }

        protected override void DataOperation()
        {
            //_userInfo.IsGotReportPacket = true;
            var fullKey = new NatUserInfoRedisKey()
            {
                ServerID = NNServerFactory.Server.ServerID,
                RemoteIPEndPoint = _session.RemoteIPEndPoint,
                PortType = _request.PortType,
                Cookie = _request.Cookie
            };

            try
            {
                _userInfo = NatUserInfoRedisOperator.GetSpecificValue(fullKey);
            }
            catch
            {
                _result.ErrorCode = NNErrorCode.ReportPacketError;
            }

            if (_request.NatResult != NATNegotiationResult.Success)
            {
                foreach (NatPortType portType in Enum.GetValues(typeof(NatPortType)))
                {
                    NatNegotiateManager.Negotiate(portType, _request.Version, _request.Cookie);
                }

                _userInfo.RetryNATNegotiationTime++;
                NatUserInfoRedisOperator.SetKeyValue(fullKey, _userInfo);
            }
            else
            {
                // natnegotiation successed we delete the negotiator
                NatUserInfoRedisOperator.DeleteKeyValue(fullKey);
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new ReportResponse(_request, _result);
        }
    }
}
