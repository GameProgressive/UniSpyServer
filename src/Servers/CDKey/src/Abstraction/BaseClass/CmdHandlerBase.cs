using UniSpyServer.CDkey.Entity.Structure.Result;
using UniSpyServer.CDkey.Network;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.CDkey.Abstraction.BaseClass
{
    public abstract class CmdHandlerBase : UniSpyServer.UniSpyLib.Abstraction.BaseClass.UniSpyCmdHandlerBase
    {
        protected new Session _session => (Session)base._session;
        protected new RequestBase _request => (RequestBase)base._request;
        protected new ResponseBase _response => (ResponseBase)base._response;
        protected new ResultBase _result
        {
            get => (ResultBase)base._result;
            set => base._result = value;
        }
        public CmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
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
