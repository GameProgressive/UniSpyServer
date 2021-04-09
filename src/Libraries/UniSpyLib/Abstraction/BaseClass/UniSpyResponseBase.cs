using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class UniSpyResponseBase : IUniSpyResponse
    {
        /// <summary>
        /// Represents the plaintext response data
        /// </summary>
        public object SendingBuffer { get; protected set; }
        protected UniSpyResultBase _result { get; }
        protected UniSpyRequestBase _request { get; }
        public UniSpyResponseBase(UniSpyRequestBase request, UniSpyResultBase result)
        {
            _request = request;
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
