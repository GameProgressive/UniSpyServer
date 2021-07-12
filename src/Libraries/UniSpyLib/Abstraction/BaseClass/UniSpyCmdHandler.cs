using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class UniSpyCmdHandler : IUniSpyHandler
    {
        protected IUniSpySession _session { get; }
        protected IUniSpyRequest _request { get; }
        protected IUniSpyResponse? _response { get; set; }
        protected UniSpyResult? _result { get; set; }
        public UniSpyCmdHandler(IUniSpySession session, IUniSpyRequest request)
        {
            _session = session;
            _request = request;
            LogWriter.LogCurrentClass(this);
        }

        public abstract void Handle();


        protected abstract void RequestCheck();
        protected abstract void DataOperation();
        protected abstract void ResponseConstruct();
        /// <summary>
        /// The response process
        /// </summary>
        protected virtual void Response()
        {
            if (_response == null)
            {
                return;
            }
            _response.Build();
            // the SendingBuffer in default response is null
            if (_response.SendingBuffer == null)
            {
                return;
            }
            _session.SendAsync(_response.SendingBuffer);
        }
    }
}
