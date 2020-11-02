using System;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Entity.Structure.Request;

namespace NATNegotiation.Handler.CommandSwitcher
{
    public class NNRequestSerializer
    {
        public static IRequest Serialize(byte[] rawRequest)
        {
            if (rawRequest.Length < 1)
            {
                return null;
            }
            IRequest request ;
            switch ((NatPacketType)rawRequest[7])
            {
                case NatPacketType.Init:
                    request = new InitRequest(rawRequest);
                    break;
                case NatPacketType.AddressCheck:
                    request = new AddressRequest(rawRequest);
                    break;
                case NatPacketType.NatifyRequest:
                    request = new NatifyRequest(rawRequest);
                    break;
                case NatPacketType.ConnectAck:
                    request = null;
                        break;
                case NatPacketType.Report:
                    request = new ReportRequest(rawRequest);
                    break;
                case NatPacketType.ErtAck:
                    request = new ErtAckRequest(rawRequest);
                    break;
                default:
                    request = null;
                    LogWriter.UnknownDataRecieved(rawRequest);
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
