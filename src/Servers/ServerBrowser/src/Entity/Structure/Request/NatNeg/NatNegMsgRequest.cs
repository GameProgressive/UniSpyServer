using System;
using System.Linq;
using UniSpyServer.Servers.ServerBrowser.Abstraction.BaseClass;
using UniSpyServer.Servers.ServerBrowser.Entity.Enumerate;

namespace UniSpyServer.Servers.ServerBrowser.Entity.Structure.Request
{
    
    public sealed class NatNegMsgRequest : RequestBase
    {
        public uint? Cookie { get; set; }
        public byte[] NatNegMessage => RawRequest;
        public NatNegMsgRequest(byte[] rawRequest) : base(rawRequest)
        {
            CommandName = RequestType.NatNegRequest;
        }

        public override void Parse()
        {
            Cookie = BitConverter.ToUInt16(RawRequest.Skip(6).ToArray());
        }
    }
}
