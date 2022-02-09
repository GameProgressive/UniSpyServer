using System;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass
{
    public abstract class CmdHandlerBase : IHandler
    {
        protected ISession _session { get; }
        protected IRequest _request { get; }
        protected IResponse _response { get; set; }
        protected ResultBase _result { get; set; }
        public CmdHandlerBase(ISession session, IRequest request)
        {
            _session = session;
            _request = request;
            LogWriter.LogCurrentClass(this);
        }

        public virtual void Handle()
        {
            try
            {
                RequestCheck();
                DataOperation();
                ResponseConstruct();
                if (_response == null)
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
        protected virtual void RequestCheck() => _request.Parse();
        protected virtual void DataOperation() { }
        protected virtual void ResponseConstruct() { }
        /// <summary>
        /// The response process
        /// </summary>
        protected virtual void Response()
        {
            _session.Send(_response);
        }
        protected virtual void HandleException(Exception ex)
        {
            LogWriter.Error(ex.ToString());
        }
    }
}
