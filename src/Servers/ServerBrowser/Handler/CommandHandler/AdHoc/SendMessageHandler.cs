using UniSpyLib.Abstraction.Interface;
using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Entity.Structure.Packet.Request;
using ServerBrowser.Network;

namespace ServerBrowser.Handler.CommandHandler
{
    public class SendMessageHandler : SBCommandHandlerBase
    {
        private AdHocRequest _request;
        public SendMessageHandler(ISession session, byte[] recv) : base(session, recv)
        {
            _request = new AdHocRequest();
        }

        protected override void CheckRequest()
        {
            //we do not call base method because we have our own check method
            //base.CheckRequest();

            if (!_request.Parse(_recv))
            {
                _errorCode = SBErrorCode.Parse;
                return;
            }
        }

        protected override void DataOperation()
        {
            base.DataOperation();
            _session.ServerMessageList.Add(_request);
        }
    }
}
