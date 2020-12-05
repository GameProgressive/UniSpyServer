using UniSpyLib.Abstraction.Interface;
using NATNegotiation.Abstraction.BaseClass;

namespace NATNegotiation.Handler.CommandHandler
{
    public class ConnectACKHandler : NNCommandHandlerBase
    {
        public ConnectACKHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            base.DataOperation();
            _session.UserInfo.SetIsGotConnectAckPacketFlag();
        }
    }
}
