using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Server;
using GameSpyLib.Common.BaseClass;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public abstract class ChatCommandHandlerBase : CommandHandlerBase
    {
        protected ChatError _errorCode;
        protected ChatCommandBase _cmd;
        public string CommandName;
        protected string _sendingBuffer;
        protected ChatSession _session;

        public ChatCommandHandlerBase(IClient client, ChatCommandBase cmd, string sendingBuffer) : base(client)
        {
            _errorCode = ChatError.NoError;
            _cmd = cmd;
            _session = (ChatSession)client.GetInstance();
            _sendingBuffer = sendingBuffer;
        }

        public abstract void SetCommandName();

        public override void Handle()
        {
            base.Handle();

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

            Response();
        }
        public virtual void CheckRequest()
        { }
        public virtual void DataOperation()
        { }
        public virtual void ConstructResponse()
        { }
        public virtual void Response() { }

        public object GetInstance()
        { return this; }

        public string GetHandlerName()
        {
            return CommandName;
        }

    }
}
