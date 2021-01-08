using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Network;
using NATNegotiation.Entity.Structure;
using NATNegotiation.Handler.SystemHandler.Manager;
using NATNegotiation.Handler.SystemHandler;

namespace NATNegotiation.Abstraction.BaseClass
{
    /// <summary>
    /// because we are using self defined error code so we do not need
    /// to send it to client, when we detect errorCode != noerror we just log it
    /// </summary>
    public class NNCommandHandlerBase : UniSpyCmdHandlerBase
    {
        protected NNErrorCode _errorCode;
        protected byte[] _sendingBuffer;
        protected new NNSession _session
        {
            get { return (NNSession)base._session; }
        }
        protected new NNRequestBase _request
        {
            get { return (NNRequestBase)base._request; }
        }

        public NNCommandHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _errorCode = NNErrorCode.NoError;
        }

        public override void Handle()
        {
            RequestCheck();
            if (_errorCode != NNErrorCode.NoError)
            {
                return;
            }

            DataOperation();
            if (_errorCode != NNErrorCode.NoError)
            {
                return;
            }
            ResponseConstruct();
            if (_errorCode != NNErrorCode.NoError)
            {
                return;
            }
            Response();
        }

        protected override void Response()
        {
            if (!StringExtensions.CheckResponseValidation(_sendingBuffer))
            {
                return;
            }
            _session.Send(_sendingBuffer);
        }
    }
}
