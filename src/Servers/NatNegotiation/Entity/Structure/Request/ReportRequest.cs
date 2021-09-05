using NatNegotiation.Abstraction.BaseClass;
using NatNegotiation.Entity.Contract;
using NatNegotiation.Entity.Enumerate;
using System;
using UniSpyLib.Encryption;
using UniSpyLib.Extensions;

namespace NatNegotiation.Entity.Structure.Request
{
    [RequestContract(RequestType.Report)]
    internal sealed class ReportRequest : RequestBase
    {
        public NatPortType PortType { get; set; }
        public byte ClientIndex { get; set; }
        public NatNegResult NatResult { get; set; }
        public NatNegType NatType { get; set; }
        public NatMappingScheme MappingScheme { get; set; }
        public string GameName { get; set; }

        public ReportRequest(byte[] rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();

            PortType = (NatPortType)RawRequest[13];
            ClientIndex = RawRequest[14];

            NatResult = (NatNegResult)RawRequest[15];

            NatType = (NatNegType)BitConverter.ToInt32(
                ByteTools.SubBytes(RawRequest, 17, sizeof(int)));
            MappingScheme = (NatMappingScheme)BitConverter.ToInt32(
                ByteTools.SubBytes(RawRequest, 19, sizeof(int)));
            GameName = UniSpyEncoding.GetString(
                ByteTools.SubBytes(RawRequest, 23, RawRequest.Length - 23));
        }
    }
}
