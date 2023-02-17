using System;
using System.Linq;
using UniSpy.Server.NatNegotiation.Abstraction.BaseClass;
using UniSpy.Server.NatNegotiation.Entity.Enumerate;
using UniSpy.Server.Core.Encryption;

namespace UniSpy.Server.NatNegotiation.Entity.Structure.Request
{

    public sealed class ReportRequest : CommonRequestBase
    {
        public bool? IsNatSuccess { get; private set; }
        public new RequestType CommandName { get => (RequestType)base.CommandName; set => base.CommandName = value; }
        public string GameName { get; private set; }
        public NatType NatType { get; private set; }
        public NatPortMappingScheme? MappingScheme { get; private set; }
        public ReportRequest(byte[] rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            // base.Parse();
            if (RawRequest.Length < 12)
            {
                return;
            }
            Version = RawRequest[6];
            CommandName = (RequestType)RawRequest[7];
            Cookie = BitConverter.ToUInt32(RawRequest.Skip(8).Take(4).ToArray());
            // port type is set 204 by gamespy
            PortType = (NatPortType)RawRequest[12];

            ClientIndex = (NatClientIndex)RawRequest[13];
            if (RawRequest[14] == 0)
            {
                IsNatSuccess = false;
            }
            else
            {
                IsNatSuccess = true;
            }
            NatType = (NatType)RawRequest[15];
            MappingScheme = (NatPortMappingScheme)RawRequest[17];
            // initpacket is 23 long, so there are 0x00 before gamename, we need to skip it
            var endIndex = Array.FindIndex(RawRequest.Skip(23).ToArray(), 1, k => k == 0);
            GameName = UniSpyEncoding.GetString(RawRequest.Skip(23).Take(endIndex).ToArray());
        }
    }
}
