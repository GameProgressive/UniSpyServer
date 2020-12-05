using UniSpyLib.Abstraction.Interface;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Entity.Structure.Request;
using NATNegotiation.Entity.Structure.Response;
using NATNegotiation.Handler.SystemHandler.Manager;

namespace NATNegotiation.Handler.CommandHandler
{
    /// <summary>
    /// Get nat neg result report success or fail
    /// </summary>
    public class ReportHandler : NNCommandHandlerBase
    {
        protected new ReportRequest _request;
        public ReportHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _request = (ReportRequest)request;
        }

        protected override void DataOperation()
        {
            _session.UserInfo.SetIsGotReportPacketFlag();

            if (_request.NatResult != NATNegotiationResult.Success)
            {
                NNManager.Negotiate(NatPortType.GP, _request.Version, _request.Cookie);
                NNManager.Negotiate(NatPortType.NN1, _request.Version, _request.Cookie);
                NNManager.Negotiate(NatPortType.NN2, _request.Version, _request.Cookie);
                NNManager.Negotiate(NatPortType.NN3, _request.Version, _request.Cookie);
            }
        }

        protected override void ConstructResponse()
        {
            _sendingBuffer = new ReportResponse(_request).BuildResponse();
        }
    }
}
