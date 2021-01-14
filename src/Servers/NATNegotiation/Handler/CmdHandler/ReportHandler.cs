using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Entity.Structure;
using NATNegotiation.Entity.Structure.Request;
using NATNegotiation.Entity.Structure.Response;
using NATNegotiation.Entity.Structure.Result;
using NATNegotiation.Handler.SystemHandler.Manager;
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
        protected NatUserInfo _userInfo;
        protected string _fullKey;
        public ReportHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
        protected override void RequestCheck()
        {
            _result = new ReportResult();
        }
        protected override void DataOperation()
        {
            //_userInfo.IsGotReportPacket = true;

            var _fullKey = NatUserInfo.RedisOperator.BuildFullKey(_session.RemoteIPEndPoint, _request.PortType, _request.Cookie);
            try
            {
                _userInfo = NatUserInfo.RedisOperator.GetSpecificValue(_fullKey);
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
                NatUserInfo.RedisOperator.SetKeyValue(_fullKey, _userInfo);
            }
            else
            {
                // natnegotiation successed we delete the negotiator
                NatUserInfo.RedisOperator.DeleteKeyValue(_fullKey);
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new ReportResponse(_request, _result);
        }
    }
}
