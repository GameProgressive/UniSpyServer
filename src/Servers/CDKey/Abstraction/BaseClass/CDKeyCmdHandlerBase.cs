using CDKey.Entity.Enumerate;
using CDKey.Entity.Structure.Result;
using CDKey.Network;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace CDKey.Abstraction.BaseClass
{
    internal abstract class CDKeyCmdHandlerBase : UniSpyCmdHandlerBase
    {
        protected new CDKeySession _session => (CDKeySession)base._session;
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
            if (_result.ErrorCode != CDKeyErrorCode.NoError)
            {
                ResponseConstruct();
                Response();
            }

            DataOperation();
            if (_result.ErrorCode != CDKeyErrorCode.NoError)
            {
                ResponseConstruct();
                Response();
            }

            ResponseConstruct();
            Response();
        }
    }
}
