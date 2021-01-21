using CDKey.Entity.Enumerate;
using CDKey.Network;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;

namespace CDKey.Abstraction.BaseClass
{
    internal abstract class CDKeyCmdHandlerBase : UniSpyCmdHandlerBase
    {
        protected new CDKeySession _session => (CDKeySession)base._session;
        protected new CDKeyRequestBase _request => (CDKeyRequestBase)base._request;
        protected new CDKeyResultBase _result
        {
            get => (CDKeyResultBase)base._result;
            set => base._result = value;
        }
        public CDKeyCmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
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


        protected override void Response()
        {
            if (_response == null)
            {
                return;
            }
            _response.Build();
            if (!StringExtensions.CheckResponseValidation((string)_response.SendingBuffer))
            {
                return;
            }
            _session.SendAsync((string)_response.SendingBuffer);
        }
    }
}
