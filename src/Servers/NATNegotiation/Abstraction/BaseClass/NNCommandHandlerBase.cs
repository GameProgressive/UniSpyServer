using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Network;

namespace NATNegotiation.Abstraction.BaseClass
{
    /// <summary>
    /// because we are using self defined error code so we do not need
    /// to send it to client, when we detect errorCode != noerror we just log it
    /// </summary>
    public abstract class NNCmdHandlerBase : UniSpyCmdHandlerBase
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
        
        public NNCmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
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

        protected override void DataOperation()
        {
            //currently we do nothing here
        }

        protected override void Response()
        {
            if (_response == null)
            {
                return;
            }

            if (!StringExtensions.CheckResponseValidation((byte[])_response.SendingBuffer))
            {
                return;
            }
            _session.Send((byte[])_response.SendingBuffer);
        }
    }
}
