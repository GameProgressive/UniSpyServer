using UniSpyLib.Abstraction.Interface;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Structure;

namespace NATNegotiation.Handler.CmdHandler
{
    public class ConnectACKHandler : NNCommandHandlerBase
    {
        public ConnectACKHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            base.DataOperation();
            _userInfo.IsGotConnectAckPacket = true;
            NatUserInfo.SetNatUserInfo(_session.RemoteEndPoint, _request.Cookie, _userInfo);
        }
    }
}
