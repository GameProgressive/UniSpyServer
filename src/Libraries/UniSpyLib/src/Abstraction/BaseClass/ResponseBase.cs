using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass
{
    public abstract class ResponseBase : IResponse
    {
        /// <summary>
        /// Represents the plaintext response data
        /// </summary>
        public object SendingBuffer { get; protected set; }
        protected ResultBase _result { get; }
        protected RequestBase _request { get; }
        public ResponseBase(RequestBase request, ResultBase result)
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
