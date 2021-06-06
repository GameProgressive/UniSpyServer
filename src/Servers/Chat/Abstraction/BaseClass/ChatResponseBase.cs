using Chat.Entity.Structure;
using Serilog.Events;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Logging;

namespace Chat.Abstraction.BaseClass
{
    internal abstract class ChatResponseBase : UniSpyResponseBase
    {
        public new string SendingBuffer
        {
            get => (string)base.SendingBuffer;
            protected set => base.SendingBuffer = value;
        }
        protected new ChatResultBase _result => (ChatResultBase)base._result;
        protected new ChatRequestBase _request => (ChatRequestBase)base._request;
        protected ChatResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            BuildNormalResponse();
        }
    }
}
