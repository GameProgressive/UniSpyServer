using UniSpyLib.Abstraction.Interface;
using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Structure.Packet.Request;

namespace ServerBrowser.Handler.CommandHandler
{
    public class SendMessageHandler : SBCommandHandlerBase
    {
        protected new AdHocRequest _request;
        public SendMessageHandler(ISession session, IRequest request) : base(session, request)
        {
            _request = (AdHocRequest)request;
        }

        protected override void DataOperation()
        {
            base.DataOperation();
            _session.ServerMessageList.Add(_request);
        }
    }
}
