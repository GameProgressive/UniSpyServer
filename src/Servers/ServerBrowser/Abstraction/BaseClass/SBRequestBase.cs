using ServerBrowser.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Extensions;

namespace ServerBrowser.Abstraction.BaseClass
{
    internal abstract class SBRequestBase : UniSpyRequestBase
    {
        public int RequestLength { get; private set; }
        public new byte[] RawRequest => (byte[])base.RawRequest;
        public new SBClientRequestType CommandName
        {
            get => (SBClientRequestType)base.CommandName;
            protected set => base.CommandName = value;
        }
        public SBRequestBase(object rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            RequestLength = ByteTools.ToUInt16(ByteTools.SubBytes(RawRequest, 0, 2), true);
        }
    }
}
