using CDKey.Entity.Structure.Result;
using CDKey.Network;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace CDKey.Abstraction.BaseClass
{
    public abstract class CmdHandlerBase : UniSpyLib.Abstraction.BaseClass.UniSpyCmdHandlerBase
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
