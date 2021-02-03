using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Entity.Structure.Response;
using NATNegotiation.Entity.Structure.Result;
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
    internal abstract class NNCmdHandlerBase : UniSpyCmdHandlerBase
    {
        protected new NNSession _session => (NNSession)base._session;
        protected new NNRequestBase _request => (NNRequestBase)base._request;
        protected new NNResultBase _result
        {
            get { return (NNResultBase)base._result; }
            set { base._result = value; }
        }
        public NNCmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new NNDefaultResult();
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
            Response();
        }
        protected override void RequestCheck()
        {
        }
        protected override void DataOperation()
        {
        }
        protected override void ResponseConstruct()
        {
            _response = new NNDefaultResponse(_request, _result);
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
