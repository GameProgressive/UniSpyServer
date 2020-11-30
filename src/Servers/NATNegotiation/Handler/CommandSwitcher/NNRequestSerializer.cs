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

        public override void Serialize()
        {
            if (_rawRequest.Length < 1)
                return;
            IRequest request = GenerateRequest(_rawRequest);
            Requests.Add(request);
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
                    request = new InitRequest(_rawRequest);
                    break;
                case NatPacketType.AddressCheck:
                    request = new AddressRequest(_rawRequest);
                    break;
                case NatPacketType.NatifyRequest:
                    request = new NatifyRequest(_rawRequest);
                    break;
                case NatPacketType.ConnectAck:
                    request = null;
                    break;
                case NatPacketType.Report:
                    request = new ReportRequest(_rawRequest);
                    break;
                case NatPacketType.ErtAck:
                    request = new ErtAckRequest(_rawRequest);
                    break;
                default:
                    request = null;
                    LogWriter.UnknownDataRecieved(_rawRequest);
                    break;
            }

            if (request == null)
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
