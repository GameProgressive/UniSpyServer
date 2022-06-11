using System;
using System.Linq;
using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;

using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.UniSpyLib.Encryption;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request
{
    
    public sealed class ReportRequest : CommonRequestBase
    {
        public NatNegResult? NatResult { get; private set; }
        public new RequestType CommandName { get => (RequestType)base.CommandName; set => base.CommandName = value; }
        public NatPortMappingScheme? MappingScheme { get; private set; }
        public string GameName { get; private set; }
        public ReportRequest(byte[] rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            PortType = (NatPortType)RawRequest[13];
            ClientIndex = (NatClientIndex)RawRequest[14];
            NatResult = (NatNegResult)RawRequest[15];
            CommandName = (RequestType)BitConverter.ToUInt16(RawRequest.Skip(17).Take(2).ToArray());
            MappingScheme = (NatPortMappingScheme)BitConverter.ToInt32(RawRequest.Skip(19).Take(4).ToArray());
            var endIndex = Array.FindIndex(RawRequest.Skip(23).ToArray(), 1, k => k == 0);
            GameName = UniSpyEncoding.GetString(RawRequest.Skip(23).Take(endIndex).ToArray());
        }
    }
}
