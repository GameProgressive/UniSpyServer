using System;
using Chat.Entity.Structure;

namespace Chat.Handler.CommandHandler
{
    public class CommandHandlerBase
    {
        protected string _sendingBuffer;
        protected ChatError _errorCode = ChatError.NoError;
        public CommandHandlerBase()
        {
        }
        public virtual void Handle(ChatSession session, string[] recv)
        {
            CheckRequest(session, recv);
            if (_errorCode != ChatError.NoError)
            {
                return;
            }

            DataOperation(session, recv);
            if (_errorCode != ChatError.NoError)
            {
                return;
            }

            ConstructResponse(session, recv);
            if (_errorCode != ChatError.NoError)
            {
                return;
            }

            Response(session, recv);
        }
        public virtual void CheckRequest(ChatSession session, string[] recv)
        { }
        public virtual void DataOperation(ChatSession session, string[] recv)
        { }
        public virtual void ConstructResponse(ChatSession session, string[] recv)
        { }

        public virtual void Response(ChatSession session, string[] recv)
        {
            if (_sendingBuffer == null)
            {
                return;
            }
            session.SendAsync(_sendingBuffer);
        }
    }
}
