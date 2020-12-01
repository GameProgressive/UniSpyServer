using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Handler.CommandHandler;
using UniSpyLib.Abstraction.BaseClass;

namespace NATNegotiation.Handler.CommandSwitcher
{
    public class NNCommandSerializer : CommandHandlerSerializerBase
    {
        protected new byte[] _rawRequest;
        public NNCommandSerializer(ISession session, object rawRequest) : base(session, rawRequest)
        {
        }

        public override void Serialize()
        {
            
        }

        protected override void SerializeCommands()
        {
            base.SerializeCommands();
            foreach (var request in _requests)
            {
                IHandler handler;
                switch (request.CommandName)
                {
                    case NatPacketType.Init:
                        handler = new InitHandler(_session, request);
                        break;
                    case NatPacketType.AddressCheck:
                        handler = new AddressCheckHandler(_session, request);
                        break;
                    case NatPacketType.NatifyRequest:
                        handler = new NatifyHandler(_session, request);
                        break;
                    case NatPacketType.ConnectAck:
                        handler = new ConnectACKHandler(_session, request);
                        break;
                    case NatPacketType.Report:
                        handler = new ReportHandler(_session, request);
                        break;
                    case NatPacketType.ErtAck:
                        handler = new ErtAckHandler(_session, request);
                        break;
                    default:
                        LogWriter.UnknownDataRecieved(_rawRequest);
                        break;
                }
            }
        }

        protected override void SerializeRequests()
        {
            base.SerializeRequests();
            var request = new NNRequestSerializer(_session, _rawRequest).Serialize();
            if (!(bool)request.Parse())
            {
                return;
            }

            _requests.Add(request);
        }
    }
}
