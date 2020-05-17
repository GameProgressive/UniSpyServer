using GameSpyLib.Common.Entity.Interface;
using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure.Packet;
using NatNegotiation.Handler.SystemHandler.NatNegotiatorManage;

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
        }

        protected override void CheckRequest()
        {
            _reportPacket = new ReportPacket();
            _reportPacket.Parse(_recv);
        }

        protected override void DataOperation()
        {
            _session.UserInfo.SetIsGotReportPacketFlag();

            if (_reportPacket.NatResult != NatNegotiationResult.Success)
            {
                NatNegotiatorPool.FindNatNegotiatorsAndSendConnectPacket(NatPortType.GP, _reportPacket.Version, _reportPacket.Cookie);
                NatNegotiatorPool.FindNatNegotiatorsAndSendConnectPacket(NatPortType.NN1, _reportPacket.Version, _reportPacket.Cookie);
                NatNegotiatorPool.FindNatNegotiatorsAndSendConnectPacket(NatPortType.NN2, _reportPacket.Version, _reportPacket.Cookie);
                NatNegotiatorPool.FindNatNegotiatorsAndSendConnectPacket(NatPortType.NN3, _reportPacket.Version, _reportPacket.Cookie);
            }
        }

        protected override void ConstructResponse()
        {
            _sendingBuffer = _reportPacket.BuildResponse();
        }
    }
}
