using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Core.Abstraction.BaseClass
{
    public abstract class ResponseBase : IResponse
    {
        /// <summary>
        /// Represents the plaintext response data
        /// </summary>
        public object SendingBuffer { get; protected set; }

        /*       
        /// <summary>
        /// Create new inhereted type of this property to make access easier without type convertion every time
        /// </summary>
        public ResultBase Result => _result;
        /// <summary>
        /// Create new inhereted type of this property to make access easier without type convertion every time
        /// </summary>
        public RequestBase Request => _request;
        */

        protected ResultBase _result { get; }
        protected RequestBase _request { get; }
        protected IClient _client { get; }
        public ResponseBase(RequestBase request, ResultBase result)
        {
            _request = request;
            _result = result;
        }

        /// <summary>
        /// Build response message
        /// </summary>
        public abstract void Build();
    }
}
