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
        protected IClient _client { get; }
        public ResponseBase(IClient client, RequestBase request, ResultBase result)
        {
            _client = client;
            _request = request;
            _result = result;
        }
        public ResponseBase(RequestBase request, ResultBase result)
        {
            _request = request;
            _result = result;
        }

        /// <summary>
        /// Build response message
        /// </summary>
        public abstract void Build();
        /// <summary>
        /// Assemble all information to build plaintext response
        /// </summary>
        public virtual void Assemble()
        {
            Build();
            if (_client.Crypto is not null)
            {
                SendingBuffer = _client.Crypto.Encrypt(SendingBuffer as byte[]);
            }
        }
    }
}
