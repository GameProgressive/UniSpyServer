using PresenceConnectionManager.Entity.Structure.Response;
using PresenceConnectionManager.Entity.Structure.Result;
using PresenceConnectionManager.Network;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;
using UniSpyLib.Logging;

namespace PresenceConnectionManager.Abstraction.BaseClass
{
    /// <summary>
    /// Because all errors are sent by SendGPError()
    /// so we if the error code != noerror we send it
    /// </summary>
    internal abstract class PCMCmdHandlerBase : UniSpyCmdHandlerBase
    {
        protected new PCMSession _session => (PCMSession)base._session;
        protected new PCMRequestBase _request => (PCMRequestBase)base._request;
        protected new PCMResultBase _result
        {
            get => (PCMResultBase)base._result;
            set => base._result = value;
        }
        public PCMCmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new PCMDefaultResult();
        }

        public override void Handle()
        {
            try
            {
                RequestCheck();
                DataOperation();
                ResponseConstruct();
                Response();
            }
            catch (GPExceptionBase e)
            {
                _session.SendAsync(e.ErrorResponse);
            }
        }

        protected override void RequestCheck()
        {
        }

        protected override void ResponseConstruct()
        {
            _response = new PCMDefaultResponse(_request, _result);
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
