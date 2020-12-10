using System;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Entity.Structure.Request;
using UniSpyLib.Abstraction.BaseClass;

namespace NATNegotiation.Handler.CmdSwitcher
{
    public class NNRequestSerializer : UniSpyRequestSerializerBase
    {
        protected new byte[] _rawRequest;
        public NNRequestSerializer(object rawRequest) : base(rawRequest)
        {
            _rawRequest = (byte[])rawRequest;
        }

        public override IUniSpyRequest Serialize()
        {
            switch ((NatPacketType)_rawRequest[7])
            {
                case NatPacketType.Init:
                    return new InitRequest(_rawRequest);
                case NatPacketType.AddressCheck:
                    return new AddressRequest(_rawRequest);
                case NatPacketType.NatifyRequest:
                    return new NatifyRequest(_rawRequest);
                case NatPacketType.ConnectAck:
                    throw new NotImplementedException();
                case NatPacketType.Report:
                    return new ReportRequest(_rawRequest);
                case NatPacketType.ErtAck:
                    return new ErtAckRequest(_rawRequest);
                default:
                    LogWriter.UnknownDataRecieved(_rawRequest);
                    return null;

            }
        }
    }
}
