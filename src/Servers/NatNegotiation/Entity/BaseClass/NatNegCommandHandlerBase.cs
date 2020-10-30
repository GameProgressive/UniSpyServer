using GameSpyLib.Abstraction.BaseClass;
using GameSpyLib.Abstraction.Interface;
using GameSpyLib.Extensions;
using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Server;

namespace NatNegotiation.Handler.CommandHandler
{
    /// <summary>
    /// because we are using self defined error code so we do not need
    /// to send it to client, when we detect errorCode != noerror we just log it
    /// </summary>
    public class NatNegCommandHandlerBase : CommandHandlerBase
    {
        protected NNErrorCode _errorCode;
        protected byte[] _sendingBuffer;
        protected new NatNegSession _session;
        protected byte[] _recv;

        public NatNegCommandHandlerBase(ISession session, byte[] recv) : base(session)
        {
            _recv = recv;
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
