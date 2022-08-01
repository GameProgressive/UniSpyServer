using System;
using System.Linq;
using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;

using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.UniSpyLib.Encryption;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request
{

    public sealed class ReportRequest : CommonRequestBase
    {
        public bool? IsNatSuccess { get; private set; }
        public new RequestType CommandName { get => (RequestType)base.CommandName; set => base.CommandName = value; }
        public NatPortMappingScheme? MappingScheme { get; private set; }
        public string GameName { get; private set; }
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
            ClientIndex = (NatClientIndex)RawRequest[13];
            if (RawRequest[14] == 0)
            {
                IsNatSuccess = false;
            }
            else
            {
                IsNatSuccess = true;
            }
            CommandName = (RequestType)BitConverter.ToUInt16(RawRequest.Skip(15).Take(2).ToArray());
            MappingScheme = (NatPortMappingScheme)BitConverter.ToInt32(RawRequest.Skip(17).Take(4).ToArray());
            var endIndex = Array.FindIndex(RawRequest.Skip(23).ToArray(), 1, k => k == 0);
            GameName = UniSpyEncoding.GetString(RawRequest.Skip(23).Take(endIndex).ToArray());
        }
    }
}
