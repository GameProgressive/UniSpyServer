using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Contract;
using UniSpyServer.Chat.Entity.Structure.Request.General;
using UniSpyServer.Chat.Entity.Structure.Response.General;
using UniSpyServer.Chat.Entity.Structure.Result.General;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Chat.Handler.CmdHandler.General
{
    [HandlerContract("USRIP")]
    public sealed class UserIPHandler : CmdHandlerBase
    {

        private new UserIPRequest _request => (UserIPRequest)base._request;
        private new UserIPResult _result
        {
            get => (UserIPResult)base._result;
            set => base._result = value;
        }
        public UserIPHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new UserIPResult();
        }

        protected override void DataOperation()
        {
            _result.RemoteIPAddress = _session.RemoteIPEndPoint.Address.ToString();

        }
        protected override void ResponseConstruct()
        {
            _response = new UserIPResponse(_request, _result);
        }
    }
}
