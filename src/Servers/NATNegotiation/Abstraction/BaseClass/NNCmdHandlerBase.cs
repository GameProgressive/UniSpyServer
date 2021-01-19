using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Network;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;

namespace NATNegotiation.Abstraction.BaseClass
{
    /// <summary>
    /// because we are using self defined error code so we do not need
    /// to send it to client, when we detect errorCode != noerror we just log it
    /// </summary>
    public abstract class NNCmdHandlerBase : UniSpyCmdHandlerBase
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
        protected new NNResultBase _result
        {
            get { return (NNResultBase)base._result; }
            set { base._result = value; }
        }
        public NNCmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        public override void Handle()
        {
            RequestCheck();
            if (_result.ErrorCode != NNErrorCode.NoError)
            {
                ResponseConstruct();
                Response();
                return;
            }

            DataOperation();
            if (_result.ErrorCode != NNErrorCode.NoError)
            {
                ResponseConstruct();
                Response();
                return;
            }

            ResponseConstruct();
            if (_result.ErrorCode != NNErrorCode.NoError)
            {
                ResponseConstruct();
                Response();
                return;
            }
            Response();
        }
        protected override void RequestCheck()
        {
        }
        protected override void DataOperation()
        {
        }
        protected override void Response()
        {
            if (_response == null)
            {
                return;
            }
            _response.Build();
            if (!StringExtensions.CheckResponseValidation((byte[])_response.SendingBuffer))
            {
                return;
            }
            _session.Send((byte[])_response.SendingBuffer);
        }
    }
}
