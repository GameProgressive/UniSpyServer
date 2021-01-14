using PresenceSearchPlayer.Entity.Enumerate;
using PresenceSearchPlayer.Entity.Structure.Response;
using PresenceSearchPlayer.Entity.Structure.Result;
using PresenceSearchPlayer.Network;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;

namespace PresenceSearchPlayer.Abstraction.BaseClass
{
    public abstract class PSPCmdHandlerBase : UniSpyCmdHandlerBase
    {
        /// <summary>
        /// Be careful the return of query function should be List type,
        /// the decision formula should use _result.Count==0
        /// </summary>
        protected new PSPRequestBase _request
        {
            get { return (PSPRequestBase)base._request; }
        }
        protected new PSPSession _session
        {
            get { return (PSPSession)base._session; }
        }
        protected new PSPResultBase _result
        {
            get { return (PSPResultBase)base._result; }
            set { base._result = value; }
        }
        public PSPCmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        public override void Handle()
        {
            RequestCheck();

            if (_result.ErrorCode < GPErrorCode.NoError)
            {
                ResponseConstruct();
                Response();
                return;
            }

            DataOperation();

            if (_result.ErrorCode < GPErrorCode.NoError)
            {
                ResponseConstruct();
                Response();
                return;
            }

            ResponseConstruct();
            Response();
        }

        protected override void RequestCheck()
        {
            _result = new PSPBasicResult();
        }

        protected override void ResponseConstruct()
        {
            _response = new PSPBasicResponse(_request, _result);
        }

        protected override void Response()
        {
            if (_response == null)
            {
                return;
            }
            _response.Build();
            // if _response is null the exception will be throw
            if (!StringExtensions.CheckResponseValidation((string)_response.SendingBuffer))
            {
                return;
            }
            _session.SendAsync((string)_response.SendingBuffer);
        }

    }
}
