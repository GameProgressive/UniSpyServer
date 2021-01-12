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
<<<<<<< HEAD
    internal abstract class NNCommandHandlerBase : UniSpyCmdHandlerBase
=======
    public abstract class NNCmdHandlerBase : UniSpyCmdHandlerBase
>>>>>>> c309f4b009e514a1d1f13db4317bdf0d8c2e4797
    {
        protected byte[] _sendingBuffer;
        protected new NNSession _session
        {
            get { return (NNSession)base._session; }
        }
        protected new NNRequestBase _request
        {
            get { return (NNRequestBase)base._request; }
        }
<<<<<<< HEAD
        protected new NNResultBase
        public NNCommandHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
=======
        
        public NNCmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
>>>>>>> c309f4b009e514a1d1f13db4317bdf0d8c2e4797
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
