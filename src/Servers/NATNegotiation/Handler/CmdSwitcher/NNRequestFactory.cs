using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Entity.Structure.Request;
using System;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace NATNegotiation.Handler.CmdSwitcher
{
    internal sealed class NNRequestFactory : UniSpyRequestFactoryBase
    {
        private new byte[] _rawRequest => (byte[])base._rawRequest;
        public NNRequestFactory(object rawRequest) : base(rawRequest)
        {
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
                    LogWriter.LogUnkownRequest(_rawRequest);
                    return null;
            }
        }
    }
}
