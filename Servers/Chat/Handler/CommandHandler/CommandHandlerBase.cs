using Chat.Entity.Structure;
using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Logging;

namespace Chat.Handler.CommandHandler
{
    public class CommandHandlerBase
    {
        protected string _sendingBuffer;
        protected ChatError _errorCode = ChatError.NoError;
        protected ChatSession _session;
        protected string[] _recv;
        public CommandHandlerBase(IClient client, string[] recv)
        {
            _session = (ChatSession)client.GetInstance();
            _recv = recv;
        }
        public virtual void Handle()
        {
            LogWriter.LogCurrentClass(this);

            CheckRequest();
            if (_errorCode != ChatError.NoError)
            {
                return;
            }

            DataOperation();
            if (_errorCode != ChatError.NoError)
            {
                return;
            }

            ConstructResponse();
            if (_errorCode != ChatError.NoError)
            {
                return;
            }

            Response();
        }
        public virtual void CheckRequest()
        { }
        public virtual void DataOperation()
        { }
        public virtual void ConstructResponse()
        { }

        public virtual void Response()
        {
            if (_sendingBuffer == null)
            {
                return;
            }
            _session.SendAsync(_sendingBuffer);
        }
    }
}
