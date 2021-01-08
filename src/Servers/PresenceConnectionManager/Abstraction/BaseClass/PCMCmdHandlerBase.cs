using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;
using PresenceSearchPlayer.Entity.Enumerate;
using PresenceConnectionManager.Network;
using PresenceConnectionManager.Entity.Structure.Response;
using PresenceConnectionManager.Entity.Structure.Result;

namespace PresenceConnectionManager.Abstraction.BaseClass
{
    /// <summary>
    /// Because all errors are sent by SendGPError()
    /// so we if the error code != noerror we send it
    /// </summary>
    public abstract class PCMCmdHandlerBase : UniSpyCmdHandlerBase
    {
        protected new PCMSession _session
        {
            get { return (PCMSession)base._session; }
        }
        protected new PCMRequestBase _request
        {
            get { return (PCMRequestBase)base._request; }
        }
        protected new PCMResultBase _result
        {
            get { return (PCMResultBase)base._result; }
            set { base._result = value; }
        }
        public PCMCmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        public override void Handle()
        {
            RequestCheck();

            if (_result.ErrorCode != GPErrorCode.NoError)
            {
                ResponseConstruct();
                Response();
                return;
            }

            DataOperation();

            if (_result.ErrorCode != GPErrorCode.NoError)
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
            _result = new PCMBasicResult(_request);
        }
        /// <summary>
        /// Usually we do not need to override this method
        /// </summary>
        protected override void ResponseConstruct()
        {
            _response = new PCMBasicResponse(_result);
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
            base._session.SendAsync((string)_response.SendingBuffer);
        }


    }
}
