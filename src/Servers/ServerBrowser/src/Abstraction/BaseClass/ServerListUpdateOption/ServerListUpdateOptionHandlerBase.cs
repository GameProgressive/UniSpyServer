using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Extensions;
namespace UniSpyServer.ServerBrowser.Abstraction.BaseClass
{
    public abstract class ServerListUpdateOptionHandlerBase : CmdHandlerBase
    {
        protected new ServerListUpdateOptionRequestBase _request => (ServerListUpdateOptionRequestBase)base._request;
        protected new ServerListUpdateOptionResultBase _result
        {
            get => (ServerListUpdateOptionResultBase)base._result;
            set => base._result = value;
        }
        protected new ServerListUpdateOptionResponseBase _response
        {
            get => (ServerListUpdateOptionResponseBase)base._response;
            set => base._response = value;
        }
        public ServerListUpdateOptionHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
        protected override void RequestCheck()
        {
            base.RequestCheck();
            string secretKey = DataOperationExtensions
                .GetSecretKey(_request.GameName);
            //we first check and get secrete key from database
            if (secretKey == null)
            {
                throw new System.ArgumentNullException("Can not find secretkey in database.");
            }
            _result.GameSecretKey = secretKey;
            //this is client public ip and default query port
            _result.ClientRemoteIP = _session.RemoteIPEndPoint.Address.GetAddressBytes();
            _session.GameSecretKey = secretKey;
            _session.ClientChallenge = _request.ClientChallenge;
        }
    }
}
