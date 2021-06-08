using PresenceSearchPlayer.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Abstraction.BaseClass
{
    internal abstract class PCMResponseBase : UniSpyResponseBase
    {
        protected new PCMResultBase _result => (PCMResultBase)base._result;
        protected new PCMRequestBase _request => (PCMRequestBase)base._request;
        public new string SendingBuffer
        {
            get => (string)base.SendingBuffer;
            protected set => base.SendingBuffer = value;
        }

        protected PCMResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            BuildNormalResponse();
        }
    }
}
