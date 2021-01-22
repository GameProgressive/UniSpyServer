using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Enumerate;
using System.Linq;
using UniSpyLib.Extensions;

namespace ServerBrowser.Entity.Structure.Request
{
    internal sealed class NatNegMsgRequest : SBRequestBase
    {
        public uint Cookie { get; set; }
        public byte[] NatNegMessage => RawRequest;
        public NatNegMsgRequest(object rawRequest) : base(rawRequest)
        {
            CommandName = SBClientRequestType.NatNegRequest;
        }

        public override void Parse()
        {
            Cookie = ByteTools.ToUInt16(RawRequest.Skip(6).ToArray());
        }
    }
}
