using UniSpyLib.Abstraction.Interface;
using NATNegotiation.Abstraction.BaseClass;

namespace NATNegotiation.Handler.CommandHandler
{
    public class ConnectACKHandler : NNCommandHandlerBase
    {
        public ConnectACKHandler(ISession session, IRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            base.DataOperation();
            _session.UserInfo.SetIsGotConnectAckPacketFlag();
        }
    }
}
