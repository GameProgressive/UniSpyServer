using UniSpyLib.Abstraction.Interface;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Entity.Structure.Request;
using NATNegotiation.Entity.Structure.Response;
using NATNegotiation.Handler.SystemHandler.Manager;
using NATNegotiation.Entity.Structure;
using System;
using NATNegotiation.Entity.Structure.Result;

namespace NATNegotiation.Handler.CmdHandler
{
    /// <summary>
    /// Get nat neg result report success or fail
    /// </summary>
<<<<<<< HEAD
    internal class ReportHandler : NNCommandHandlerBase
=======
    public class ReportHandler : NNCmdHandlerBase
>>>>>>> c309f4b009e514a1d1f13db4317bdf0d8c2e4797
    {
        protected new ReportRequest _request => (ReportRequest)base._request; 
        
        protected NatUserInfo _userInfo;
        protected string _fullKey;
        public ReportHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
        protected override void RequestCheck()
        {
<<<<<<< HEAD
=======
            _result = new ReportResult();
>>>>>>> c309f4b009e514a1d1f13db4317bdf0d8c2e4797
            var _fullKey = NatUserInfo.RedisOperator.BuildFullKey(_session.RemoteIPEndPoint, _request.PortType, _request.Cookie);
            try
            {
                _userInfo = NatUserInfo.RedisOperator.GetSpecificValue(_fullKey);
            }
            catch
            {
                _errorCode = NNErrorCode.ReportPacketError;
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
