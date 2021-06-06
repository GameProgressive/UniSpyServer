using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Enumerate;
using System;
using UniSpyLib.Encryption;
using UniSpyLib.Extensions;

namespace NATNegotiation.Entity.Structure.Request
{
    internal class ReportRequest : NNRequestBase
    {
        public NatPortType PortType;
        public byte ClientIndex;
        public NATNegotiationResult NatResult;
        public NATNegotiationType NatType;
        public NATNegotiationMappingScheme MappingScheme;
        public string GameName;

        public ReportRequest(byte[] rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();

            PortType = (NatPortType)RawRequest[13];
            ClientIndex = RawRequest[14];

            NatResult = (NATNegotiationResult)RawRequest[15];

            NatType = (NATNegotiationType)BitConverter.ToInt32(
                ByteTools.SubBytes(RawRequest, 17, sizeof(int)));
            MappingScheme = (NATNegotiationMappingScheme)BitConverter.ToInt32(
                ByteTools.SubBytes(RawRequest, 19, sizeof(int)));
            GameName = UniSpyEncoding.GetString(
                ByteTools.SubBytes(RawRequest, 23, RawRequest.Length - 23));
        }
    }
}
