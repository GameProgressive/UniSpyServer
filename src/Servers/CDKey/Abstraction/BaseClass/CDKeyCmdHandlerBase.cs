using CDKey.Entity.Structure.Result;
using CDKey.Network;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace CDKey.Abstraction.BaseClass
{
    internal abstract class CDKeyCmdHandlerBase : UniSpyCmdHandlerBase
    {
        protected new Session _session => (Session)base._session;
        protected new CDKeyRequestBase _request => (CDKeyRequestBase)base._request;
        protected new CDKeyResponseBase _response => (CDKeyResponseBase)base._response;
        protected new CDKeyResultBase _result
        {
            get => (CDKeyResultBase)base._result;
            set => base._result = value;
        }
        public CDKeyCmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
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
