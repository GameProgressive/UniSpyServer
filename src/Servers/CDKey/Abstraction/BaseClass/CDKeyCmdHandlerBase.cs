using CDKey.Network;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace CDKey.Abstraction.BaseClass
{
    internal abstract class CDKeyCmdHandlerBase : UniSpyCmdHandlerBase
    {
        protected string _sendingBuffer;
        protected new CDKeySession _session => (CDKeySession)base._session;
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
