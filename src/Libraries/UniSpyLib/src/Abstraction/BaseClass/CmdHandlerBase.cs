using System;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Encryption;
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
            byte[] buffer = null;
            _response.Build();
            if (_response.SendingBuffer.GetType() == typeof(string))
            {
                buffer = UniSpyEncoding.GetBytes((string)_response.SendingBuffer);
            }
            else
            {
                buffer = (byte[])_response.SendingBuffer;
            }

            LogWriter.LogNetworkSending(_client.Session.RemoteIPEndPoint, (byte[])buffer);

            //Encrypt the response if Crypto is not null
            buffer = EncryptMessage(buffer);
            _client.Session.Send(buffer);
        }

        protected virtual byte[] EncryptMessage(byte[] buffer)
        {
            if (_client.Crypto != null)
            {
                return _client.Crypto.Encrypt(buffer);
            }
            else
            {
                return buffer;
            }
        }

        protected virtual void HandleException(Exception ex)
        {
            LogWriter.Error(ex.ToString());
        }
    }
}
