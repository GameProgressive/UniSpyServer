using UniSpyLib.Extensions;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Enumerate;
using System;
using System.Text;

namespace NATNegotiation.Entity.Structure.Request
{
    public class ReportRequest : NNRequestBase
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
            if (!ErrorCode)
            {
                return;
            }

            PortType = (NatPortType)RawRequest[13];
            ClientIndex = RawRequest[14];

            NatResult = (NATNegotiationResult)RawRequest[15];

            NatType = (NATNegotiationType)BitConverter.ToInt32(
                ByteTools.SubBytes(RawRequest, 17, sizeof(int)));
            MappingScheme = (NATNegotiationMappingScheme)BitConverter.ToInt32(
                ByteTools.SubBytes(RawRequest, 19, sizeof(int)));
            GameName = Encoding.ASCII.GetString(
                ByteTools.SubBytes(RawRequest, 23, RawRequest.Length - 23));

            ErrorCode = true;
        }
    }
}
