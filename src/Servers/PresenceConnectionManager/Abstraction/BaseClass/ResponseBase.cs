using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Abstraction.BaseClass
{
    internal abstract class ResponseBase : UniSpyResponseBase
    {
        protected new PCMResultBase _result => (PCMResultBase)base._result;
        protected new RequestBase _request => (RequestBase)base._request;
        public new string SendingBuffer
        {
            get => (string)base.SendingBuffer;
            protected set => base.SendingBuffer = value;
        }

        protected ResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
    }
}
