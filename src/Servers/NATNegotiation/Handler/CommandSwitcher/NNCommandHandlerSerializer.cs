using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Handler.CommandHandler;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace NATNegotiation.Handler.CommandSwitcher
{
    public class NNCommandHandlerSerializer : CommandHandlerSerializerBase
    {
        public NNCommandHandlerSerializer(IUniSpySession session, IUniSpyRequest request) : base(session, request)
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
                case NatPacketType.ConnectAck:
                    return new ConnectACKHandler(_session, _request);
                case NatPacketType.Report:
                    return new ReportHandler(_session, _request);
                case NatPacketType.ErtAck:
                    return new ErtAckHandler(_session, _request);
                default:
                    //LogWriter.UnknownDataRecieved(_request.RawRequest);
                    return null;
            }
        }
    }
}
