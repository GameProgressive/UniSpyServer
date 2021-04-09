using System;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class UniSpyCmdHandlerBase : IUniSpyHandler
    {
        protected IUniSpySession _session { get; }
        protected IUniSpyRequest _request { get; }
        protected IUniSpyResponse _response { get; set; }
        protected UniSpyResultBase _result { get; set; }
        /// <summary>
        /// Encrypted sending buffer
        /// </summary>
        protected object _sendingBuffer { get; set; }
        public UniSpyCmdHandlerBase(IUniSpySession session, IUniSpyRequest request)
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
            LogNetworkTraffic();
            Encrypt();

            if (_sendingBuffer.GetType().Equals(typeof(byte[])))
            {
                _session.SendAsync((byte[])_sendingBuffer);
            }
            else if (_sendingBuffer.GetType().Equals(typeof(string)))
            {
                _session.SendAsync((string)_sendingBuffer);
            }
        }
        /// <summary>
        /// Encrypt message
        /// </summary>
        protected virtual void Encrypt()
        {
            _sendingBuffer = _response.SendingBuffer;
        }

        private void LogNetworkTraffic()
        {
            if (_sendingBuffer.GetType().Equals(typeof(byte[])))
            {
                LogWriter.LogNetworkSending(_session.RemoteIPEndPoint, (byte[])_response.SendingBuffer);
            }
            else if (_sendingBuffer.GetType().Equals(typeof(string)))
            {
                LogWriter.LogNetworkSending(_session.RemoteIPEndPoint, (string)_response.SendingBuffer);
            }
            else
            {
                throw new FormatException("SendingBuffer is an unknown type");
            }
        }
    }
}
