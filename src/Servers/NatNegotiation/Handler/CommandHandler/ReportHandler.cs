using GameSpyLib.Abstraction.Interface;
using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure.Packet;
using NatNegotiation.Handler.SystemHandler.NatNegotiationManage;

namespace NatNegotiation.Handler.CommandHandler
{
    /// <summary>
    /// Get nat neg result report success or fail
    /// </summary>
    public class ReportHandler : NatNegCommandHandlerBase
    {
        protected ReportPacket _reportPacket;
        public ReportHandler(ISession session, byte[] recv) : base(session, recv)
        {
            _reportPacket = new ReportPacket();
        }

        protected override void CheckRequest()
        {
            _reportPacket.Parse(_recv);
        }

        protected override void DataOperation()
        {
            _session.UserInfo.SetIsGotReportPacketFlag();

            if (_reportPacket.NatResult != NatNegotiationResult.Success)
            {
                NatNegotiationManager.Negotiate(NatPortType.GP, _reportPacket.Version, _reportPacket.Cookie);
                NatNegotiationManager.Negotiate(NatPortType.NN1, _reportPacket.Version, _reportPacket.Cookie);
                NatNegotiationManager.Negotiate(NatPortType.NN2, _reportPacket.Version, _reportPacket.Cookie);
                NatNegotiationManager.Negotiate(NatPortType.NN3, _reportPacket.Version, _reportPacket.Cookie);
            }
        }

        protected override void ConstructResponse()
        {
            _sendingBuffer = _reportPacket.BuildResponse();
        }
    }
}
