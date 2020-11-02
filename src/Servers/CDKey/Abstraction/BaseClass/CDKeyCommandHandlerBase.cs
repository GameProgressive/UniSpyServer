using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using System.Collections.Generic;

namespace CDKey.Abstraction.BaseClass
{
    public class CDKeyCommandHandlerBase : CommandHandlerBase
    {
        protected string _sendingBuffer;

        public CDKeyCommandHandlerBase(ISession client, Dictionary<string, string> recv) : base(client)
        {
        }

        public override void Handle()
        {
            CheckRequest();

            DataOperation();

            ConstructResponse();

            Response();
        }

        public virtual void CheckRequest()
        {
        }

        public virtual void DataOperation()
        {
        }

        public virtual void ConstructResponse()
        {
        }

        public virtual void Response()
        {
            if (_sendingBuffer == null || _sendingBuffer == "" || _sendingBuffer.Length < 3)
            {
                return;
            }
            _session.SendAsync(_sendingBuffer);
        }
    }
}
