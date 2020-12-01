using System;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Entity.Structure.Request;
using UniSpyLib.Abstraction.BaseClass;

namespace NATNegotiation.Handler.CommandSwitcher
{
    public class NNRequestSerializer : RequestSerializerBase
    {
        protected new byte[] _rawRequest;
        public NNRequestSerializer(ISession session, object rawRequest) : base(session, rawRequest)
        {
            _rawRequest = (byte[])rawRequest;
        }

        public override IRequest Serialize()
        {
            if (_rawRequest.Length < 1)
                return null;

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

        protected override IRequest GenerateRequest(object singleRequest)
        {
            if (_rawRequest.Length < 1)
            {
                return null;
            }

            IRequest request;
            switch ((NatPacketType)_rawRequest[7])
            {
                case NatPacketType.Init:
                    return new InitRequest(_rawRequest);

                case NatPacketType.AddressCheck:
                    return new AddressRequest(_rawRequest);

                case NatPacketType.NatifyRequest:
                    return new NatifyRequest(_rawRequest);

                case NatPacketType.ConnectAck:
                    return null;

                case NatPacketType.Report:
                    return new ReportRequest(_rawRequest);

                case NatPacketType.ErtAck:
                    return new ErtAckRequest(_rawRequest);

                default:
                    return null;
                    LogWriter.UnknownDataRecieved(_rawRequest);

            }

            if (return= null)
            {
                return null;
            }

            if (!(bool)request.Parse())
            {
                return null;
            }
            return request;
        }
    }
}
