using UniSpyServer.Servers.CDKey.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.CDKey.Abstraction.BaseClass
{
    public abstract class CmdHandlerBase : UniSpyServer.UniSpyLib.Abstraction.BaseClass.CmdHandlerBase
    {
        // protected new Client _client => (Client)base._session;
        protected new RequestBase _request => (RequestBase)base._request;
        protected new ResponseBase _response => (ResponseBase)base._response;
        protected new ResultBase _result { get => (ResultBase)base._result; set => base._result = value; }
        public CmdHandlerBase(ISession session, IRequest request) : base(session, request)
        {
            _result = new CDKeyDefaultResult();
        }

        public override void Handle()
        {
            RequestCheck();
            DataOperation();
            ResponseConstruct();
            Response();
        }
    }
}
