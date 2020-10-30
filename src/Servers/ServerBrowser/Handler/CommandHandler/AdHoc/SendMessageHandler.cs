using GameSpyLib.Abstraction.Interface;
using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Entity.Structure.Packet.Request;

namespace ServerBrowser.Handler.CommandHandler.AdHoc.SendMessage
{
    public class SendMessageHandler : SBCommandHandlerBase
    {
        private AdHocRequest _request;
        public SendMessageHandler(ISession client, byte[] recv) : base(client, recv)
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
            var session = (SBSession)_session.GetInstance();
            session.ServerMessageList.Add(_request);
        }
    }
}
