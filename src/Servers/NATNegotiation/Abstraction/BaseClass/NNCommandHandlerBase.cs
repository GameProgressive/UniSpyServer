using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Server;

namespace NATNegotiation.Abstraction.BaseClass
{
    /// <summary>
    /// because we are using self defined error code so we do not need
    /// to send it to client, when we detect errorCode != noerror we just log it
    /// </summary>
    public class NNCommandHandlerBase : CommandHandlerBase
    {
        protected NNErrorCode _errorCode;
        protected byte[] _sendingBuffer;
        protected new NatNegSession _session;
        protected NNRequestBase _request;

        public NNCommandHandlerBase(ISession session, IRequest request) : base(session)
        {
            _request = (NNRequestBase)request;
            _session = (NatNegSession)session.GetInstance();
            _errorCode = NNErrorCode.NoError;
        }

        public override void Handle()
        {
            base.Handle();

            CheckRequest();
            if (_errorCode != NNErrorCode.NoError)
            {
                return;
            }

            DataOperation();
            if (_errorCode != NNErrorCode.NoError)
            {
                return;
            }
            ConstructResponse();
            if (_errorCode != NNErrorCode.NoError)
            {
                return;
            }
            Response();
        }

        protected virtual void CheckRequest()
        {
            _session.UserInfo.UpdateLastPacketReceiveTime();
        }

        protected virtual void DataOperation()
        {
        }

        protected virtual void ConstructResponse()
        {
        }

        protected virtual void Response()
        {
            if (!StringExtensions.CheckResponseValidation(_sendingBuffer))
            {
                return;
            }
            _session.Send(_sendingBuffer);
        }
    }
}
