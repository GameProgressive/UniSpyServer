using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Logging;

namespace UniSpy.Server.Core.Abstraction.BaseClass
{
    public abstract class CmdHandlerBase : IHandler, ITestHandler
    {
        protected IClient _client { get; }
        protected IRequest _request { get; }
        protected ResultBase _result { get; set; }
        protected IResponse _response { get; set; }

        RequestBase ITestHandler.Request => (RequestBase)_request;
        ResultBase ITestHandler.Result => (ResultBase)_result;
        ResponseBase ITestHandler.Response => (ResponseBase)_response;

        public CmdHandlerBase(IClient client, IRequest request)
        {
            _client = client;
            _request = request;
        }
        public virtual void Handle()
        {
            try
            {
                // we first log this class
                LogCurrentClass();
                // then we handle it
                RequestCheck();
                DataOperation();
                ResponseConstruct();
                if (_response is null)
                {
                    return;
                }
                Response();
            }
            catch (UniSpy.Exception ex)
            {
                HandleException(ex);
            }
        }
        protected virtual void RequestCheck()
        {
            // if there is gamespy raw request we convert it to unispy request
            if (_request.RawRequest is not null)
            {
                _request.Parse();
            }
        }
        protected virtual void DataOperation() { }
        protected virtual void ResponseConstruct() { }
        /// <summary>
        /// The response process
        /// </summary>
        protected virtual void Response()
        {
            _client.Send(_response);
        }

        protected virtual void HandleException(System.Exception ex)
        {
            // we only log exception message when this message is UniSpy.Exception
            if (ex is UniSpy.Exception)
            {
                _client.LogError(ex.Message);
            }
            else
            {
                _client.LogError(ex.ToString());
            }
        }

        private void LogCurrentClass()
        {
            if (_client is null)
            {
                LogWriter.LogCurrentClass(this);
            }
            else
            {
                _client.LogCurrentClass(this);
            }
        }
    }
}
