using UniSpyServer.Servers.ServerBrowser.Abstraction.BaseClass;
using UniSpyServer.Servers.ServerBrowser.Entity.Enumerate;
using System.Linq;
using System;
using UniSpyServer.Servers.ServerBrowser.Entity.Contract;

namespace UniSpyServer.Servers.ServerBrowser.Entity.Structure.Request
{
    [RequestContract(RequestType.NatNegRequest)]
    public sealed class NatNegMsgRequest : RequestBase
    {
        public uint Cookie { get; set; }
        public byte[] NatNegMessage => RawRequest;
        public NatNegMsgRequest(object rawRequest) : base(rawRequest)
        {
            CommandName = RequestType.NatNegRequest;
        }

        public override void Parse()
        {
            Cookie = BitConverter.ToUInt16(RawRequest.Skip(6).ToArray());
        }
    }
}
