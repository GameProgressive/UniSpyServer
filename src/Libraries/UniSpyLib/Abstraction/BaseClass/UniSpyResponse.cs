using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class UniSpyResponse : IUniSpyResponse
    {
        /// <summary>
        /// Represents the plaintext response data
        /// </summary>
        public object SendingBuffer { get; protected set; }
        protected UniSpyResult _result { get; }
        protected UniSpyRequest _request { get; }
        public UniSpyResponse(UniSpyRequest request, UniSpyResult result)
        {
            _request = request;
            _result = result;
            LogWriter.LogCurrentClass(this);
        }

        /// <summary>
        /// Build response message
        /// </summary>
        public abstract void Build();
    }
}
