using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using System.Collections.Generic;
using CDKey.Network;

namespace CDKey.Abstraction.BaseClass
{
    public class CDKeyCommandHandlerBase : UniSpyCmdHandlerBase
    {
        protected string _sendingBuffer;
        protected new CDKeySession _session
        {
            get { return (CDKeySession)base._session; }
            set { base._session = value; }
        }
        public CDKeyCommandHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
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
