using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Handler.CommandHandler;
using UniSpyLib.Abstraction.BaseClass;

namespace NATNegotiation.Handler.CommandSwitcher
{
    public class NNCommandSerializer : CommandSerializerBase
    {
        protected new byte[] _rawRequest;
        public NNCommandSerializer(ISession session, object rawRequest) : base(session, rawRequest)
        {
        }

        public override void Serialize()
        {
            var serializer =  new NNRequestSerializer(_session, _rawRequest);
            serializer.Serialize();
            foreach(var request in serializer.Requests)
                switch (request.CommandName)
                {
                    case NatPacketType.Init:
                        new InitHandler(_session, request).Handle();
                        break;
                    case NatPacketType.AddressCheck:
                        new AddressCheckHandler(_session, request).Handle();
                        break;
                    case NatPacketType.NatifyRequest:
                        new NatifyHandler(_session, request).Handle();
                        break;
                    case NatPacketType.ConnectAck:
                        new ConnectACKHandler(_session, request).Handle();
                        break;
                    case NatPacketType.Report:
                        new ReportHandler(_session, request).Handle();
                        break;
                    case NatPacketType.ErtAck:
                        new ErtAckHandler(_session, request).Handle();
                        break;
                    default:
                        LogWriter.UnknownDataRecieved(_rawRequest);
                        break;
                }
        }
    }
}
