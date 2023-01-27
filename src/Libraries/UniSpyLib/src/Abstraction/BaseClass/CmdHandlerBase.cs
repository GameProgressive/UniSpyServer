using System;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass
{
    public abstract class CmdHandlerBase : IHandler
    {
        protected IClient _client { get; }
        protected IRequest _request { get; }
        protected IResponse _response { get; set; }
        protected ResultBase _result { get; set; }

        public CmdHandlerBase(IClient client, IRequest request)
        {
            _client = client;
            _request = request;
            _client.LogCurrentClass(this);
        }
        public virtual void Handle()
        {
            try
            {
                RequestCheck();
                DataOperation();
                ResponseConstruct();
                if (_response is null)
                {
                    return;
                }
                Response();
            }
            catch (Exception ex)
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

        protected virtual void HandleException(Exception ex)
        {
            // we only log exception message when this message is UniSpyException
            if (ex is UniSpyException)
            {
                _client.LogError(ex);
            }
            else
            {
                _client.LogError(ex.ToString());
            }
        }
    }
}
