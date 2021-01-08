using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using CDKey.Network;

namespace CDKey.Abstraction.BaseClass
{
    public abstract class CDKeyCmdHandlerBase : UniSpyCmdHandlerBase
    {
        protected string _sendingBuffer;
        protected new CDKeySession _session
        {
            get { return (CDKeySession)base._session; }
            set { base._session = value; }
        }
        public CDKeyCmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        public override void Handle()
        {
            RequestCheck();

            DataOperation();

            ResponseConstruct();

            Response();
        }

      
        protected override void Response()
        {
            if (_sendingBuffer == null || _sendingBuffer == "" || _sendingBuffer.Length < 3)
            {
                return;
            }
            _session.SendAsync(_sendingBuffer);
        }
    }
}
