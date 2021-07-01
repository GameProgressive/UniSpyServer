using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Abstraction.BaseClass
{
    internal abstract class ChatResponseBase : UniSpyResponse
    {
        public new string SendingBuffer
        {
            get => (string)base.SendingBuffer;
            protected set => base.SendingBuffer = value;
        }
        protected new ChatResultBase _result => (ChatResultBase)base._result;
        protected new ChatRequestBase _request => (ChatRequestBase)base._request;
        protected ChatResponseBase(UniSpyRequest request, UniSpyResult result) : base(request, result)
        {
        }
    }
}
