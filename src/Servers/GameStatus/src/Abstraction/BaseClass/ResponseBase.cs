using UniSpyLib.Abstraction.BaseClass;

namespace GameStatus.Abstraction.BaseClass
{
    public abstract class ResponseBase : UniSpyResponseBase
    {
        protected new RequestBase _request => (RequestBase)base._request;
        protected new ResultBase _result => (ResultBase)base._result;
        protected new string SendingBuffer
        {
            get => (string)base.SendingBuffer;
            set => base.SendingBuffer = value;
        }
        public ResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

    }
}
