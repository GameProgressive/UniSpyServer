using UniSpyLib.Abstraction.Interface;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Entity.Structure.Request;
using NATNegotiation.Entity.Structure.Response;
using NATNegotiation.Handler.SystemHandler.Manager;
using NATNegotiation.Entity.Structure;
using System;

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
        protected NatUserInfo _userInfo;

        public ReportHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
        protected override void CheckRequest()
        {
            base.CheckRequest();
            var keys = NatUserInfo.GetMatchedKeys(_session.RemoteEndPoint, _request.Cookie);
            if (keys.Count == 0)
            {
                _errorCode = NNErrorCode.ReportPacketError;
                return;
            }
            else
            {
                _userInfo = NatUserInfo.GetNatUserInfo(_session.RemoteEndPoint, _request.PortType, _request.Cookie);
            }
        }
        protected override void DataOperation()
        {
            //_userInfo.IsGotReportPacket = true;

            if (_request.NatResult != NATNegotiationResult.Success)
            {
                foreach (NatPortType portType in Enum.GetValues(typeof(NatPortType)))
                {
                    NatNegotiateManager.Negotiate(portType, _request.Version, _request.Cookie);
                }

                _userInfo.RetryNATNegotiationTime++;
                NatUserInfo.SetNatUserInfo(_session.RemoteEndPoint, _request.PortType, _request.Cookie, _userInfo);
            }
            //else
            //{
            //    // natnegotiation successed we delete the negotiator
            //    NatUserInfo.DeleteNatUserInfo(_session.RemoteEndPoint, _request.Cookie);
            //}
        }

        protected override void ConstructResponse()
        {
            _sendingBuffer = new ReportResponse(_request).BuildResponse();
        }
    }
}
