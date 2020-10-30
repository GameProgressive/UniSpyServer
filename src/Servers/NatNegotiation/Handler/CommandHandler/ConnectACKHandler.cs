using GameSpyLib.Abstraction.Interface;

namespace NatNegotiation.Handler.CommandHandler.ConnectACK
{
    public class ConnectACKHandler : NatNegCommandHandlerBase
    {
        public ConnectACKHandler(ISession session, byte[] recv) : base(session, recv)
        {
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();
        }

        protected override void ConstructResponse()
        {
            base.ConstructResponse();
        }

        protected override void DataOperation()
        {
            base.DataOperation();
            _session.UserInfo.SetIsGotConnectAckPacketFlag();
        }
    }
}
