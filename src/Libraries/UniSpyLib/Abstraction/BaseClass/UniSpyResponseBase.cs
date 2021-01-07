using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class UniSpyResponseBase : IUniSpyResponse
    {
        public object SendingBuffer { get; protected set; }
        protected UniSpyResultBase _result;
        public UniSpyResponseBase(UniSpyResultBase result)
        {
            _result = result;
            LogWriter.LogCurrentClass(this);
        }

        /// <summary>
        /// Build response message
        /// </summary>
        public abstract void Build();

        protected abstract void BuildErrorResponse();
        protected abstract void BuildNormalResponse();
    }
}
