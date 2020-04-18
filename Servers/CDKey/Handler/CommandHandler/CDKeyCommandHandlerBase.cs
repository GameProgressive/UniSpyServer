using GameSpyLib.Common.BaseClass;
using GameSpyLib.Common.Entity.Interface;
using System.Collections.Generic;

namespace CDKey.Handler.CommandHandler
{
    public class CDKeyCommandHandlerBase : CommandHandlerBase
    {
        protected string _sendingBuffer;

        public CDKeyCommandHandlerBase(IClient client, Dictionary<string, string> recv) : base(client)
        {
        }

        public override void Handle()
        {


        }

        public virtual void CheckRequest()
        {
        }

        public virtual void DataBaseOperation()
        {
        }

        public virtual void CheckDatabaseResult()
        {
        }

        public virtual void Response()
        {
            if (_sendingBuffer == null || _sendingBuffer == "")
            {
                return;
            }
            _client.SendAsync(_sendingBuffer);
        }
    }
}
