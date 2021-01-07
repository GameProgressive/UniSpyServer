using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;
using PresenceSearchPlayer.Entity.Enumerate;
using PresenceSearchPlayer.Network;

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
            _result.ErrorCode = GPErrorCode.NoError;
        }

        public override void Handle()
        {
            CheckRequest();

            if (_result.ErrorCode < GPErrorCode.NoError)
            {
                ConstructResponse();
                Response();
                return;
            }

            DataOperation();

            if (_result.ErrorCode < GPErrorCode.NoError)
            {
                ConstructResponse();
                Response();
                return;
            }

            ConstructResponse();
            Response();
        }

        /// <summary>
        /// Usually we do not need to override this function
        /// </summary>
        protected override void ConstructResponse()
        {
            _response.Build();
        }

        protected override void Response()
        {
            // if _response is null the exception will be throw
            if (!StringExtensions.CheckResponseValidation((string)_response.SendingBuffer))
            {
                return;
            }
            _session.SendAsync((string)_response.SendingBuffer);
        }

        protected override void CheckRequest() { }
    }
}
