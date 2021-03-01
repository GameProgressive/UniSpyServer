using ServerBrowser.Entity.Enumerate;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;

namespace ServerBrowser.Abstraction.BaseClass
{
    internal abstract class ServerListHandlerBase : SBCmdHandlerBase
    {
        protected new ServerListRequestBase _request => (ServerListRequestBase)base._request;
        protected new ServerListResultBase _result
        {
            get => (ServerListResultBase)base._result;
            set => base._result = value;
        }
        public ServerListHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
        protected override void RequestCheck()
        {
            string secretKey;
            //we first check and get secrete key from database
            if (!DataOperationExtensions
                .GetSecretKey(_request.GameName, out secretKey))
            {
                _result.ErrorCode = SBErrorCode.UnSupportedGame;
                return;
            }
            _result.GameSecretKey = secretKey;
            //this is client public ip and default query port
            _result.ClientRemoteIP = _session.RemoteIPEndPoint.Address.GetAddressBytes();
            _session.GameSecretKey = secretKey;
            _session.RequestChallenge = _request.Challenge;
        }
    }
}
