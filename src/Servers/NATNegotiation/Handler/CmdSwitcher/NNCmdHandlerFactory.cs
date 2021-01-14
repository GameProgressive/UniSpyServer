using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Handler.CmdHandler;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace NATNegotiation.Handler.CmdSwitcher
{
    public class NNCmdHandlerFactory : UniSpyCmdHandlerFactoryBase
    {
        public NNCmdHandlerFactory(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        public override IUniSpyHandler Serialize()
        {
            switch (_request.CommandName)
            {
                case NatPacketType.Init:
                    return new InitHandler(_session, _request);
                case NatPacketType.AddressCheck:
                    return new AddressCheckHandler(_session, _request);
                case NatPacketType.NatifyRequest:
                    return new NatifyHandler(_session, _request);
                case NatPacketType.Report:
                    return new ReportHandler(_session, _request);
                case NatPacketType.ErtAck:
                    return new ErtAckHandler(_session, _request);
                default:
                    return null;
            }
        }
    }
}
