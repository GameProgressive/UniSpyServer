using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Extensions;
namespace UniSpyServer.Servers.ServerBrowser.Abstraction.BaseClass
{
    public abstract class ServerListUpdateOptionHandlerBase : CmdHandlerBase
    {
        protected new ServerListUpdateOptionRequestBase _request => (ServerListUpdateOptionRequestBase)base._request;
        protected new ServerListUpdateOptionResultBase _result { get => (ServerListUpdateOptionResultBase)base._result; set => base._result = value; }
        protected new ServerListUpdateOptionResponseBase _response { get => (ServerListUpdateOptionResponseBase)base._response; set => base._response = value; }
        public ServerListUpdateOptionHandlerBase(ISession session, IRequest request) : base(session, request)
        {
        }
        protected override void RequestCheck()
        {
            base.RequestCheck();
            if (_session.GameSecretKey == null)
            {
                string secretKey = DataOperationExtensions
                                .GetSecretKey(_request.GameName);
                //we first check and get secrete key from database
                if (secretKey == null)
                {
                    throw new System.ArgumentNullException("Can not find secretkey in database.");
                }
                //this is client public ip and default query port
                _session.GameSecretKey = secretKey;
            }
            if (_session.ClientChallenge == null)
            {
                _session.ClientChallenge = _request.ClientChallenge;
            }
        }
    }
}
