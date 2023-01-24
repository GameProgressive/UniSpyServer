using System;
using System.Linq;
using UniSpyServer.Servers.ServerBrowser.V2.Abstraction.BaseClass;
using UniSpyServer.Servers.ServerBrowser.V2.Entity.Enumerate;

namespace UniSpyServer.Servers.ServerBrowser.V2.Entity.Structure.Request
{
    
    public sealed class NatNegMsgRequest : RequestBase
    {
        public uint? Cookie { get; set; }
        public NatNegMsgRequest(byte[] rawRequest) : base(rawRequest)
        {
            CommandName = RequestType.NatNegMsgRequest;
        }

        public override void Parse()
        {
            Cookie = BitConverter.ToUInt16(RawRequest.Skip(6).ToArray());
        }
    }
}
