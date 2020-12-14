using UniSpyLib.Abstraction.Interface;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Entity.Structure.Request;
using NATNegotiation.Entity.Structure.Response;
using NATNegotiation.Handler.SystemHandler.Manager;

namespace NATNegotiation.Handler.CmdHandler
{
    /// <summary>
    /// Get nat neg result report success or fail
    /// </summary>
    public class ReportHandler : NNCommandHandlerBase
    {
        protected new ReportRequest _request
        {
            get { return (ReportRequest)base._request; }
        }
        public ReportHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            _userInfo.IsGotReportPacket = true;

            if (_request.NatResult != NATNegotiationResult.Success)
            {
                NegotiatorManager.Negotiate(NatPortType.GP, _request.Version, _request.Cookie);
                NegotiatorManager.Negotiate(NatPortType.NN1, _request.Version, _request.Cookie);
                NegotiatorManager.Negotiate(NatPortType.NN2, _request.Version, _request.Cookie);
                NegotiatorManager.Negotiate(NatPortType.NN3, _request.Version, _request.Cookie);
            }
            NegotiatorManager.SetNatUserInfo(_session.RemoteEndPoint, _request.Cookie, _userInfo);
        }

        protected override void ConstructResponse()
        {
            _sendingBuffer = new ReportResponse(_request).BuildResponse();
        }
    }
}
