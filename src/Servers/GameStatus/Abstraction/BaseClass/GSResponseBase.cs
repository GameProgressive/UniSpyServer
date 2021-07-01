using UniSpyLib.Abstraction.BaseClass;

namespace GameStatus.Abstraction.BaseClass
{
    internal abstract class GSResponseBase : UniSpyResponse
    {
        protected new GSRequestBase _request => (GSRequestBase)base._request;
        protected new GSResultBase _result => (GSResultBase)base._result;
        protected new string SendingBuffer
        {
            get => (string)base.SendingBuffer;
            set => base.SendingBuffer = value;
        }
        public GSResponseBase(UniSpyRequest request, UniSpyResult result) : base(request, result)
        {
        }

    }
}
