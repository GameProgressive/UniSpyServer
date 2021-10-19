using ServerBrowser.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Extensions;

namespace ServerBrowser.Abstraction.BaseClass
{
    public abstract class RequestBase : UniSpyRequestBase
    {
        public int RequestLength { get; private set; }
        public new byte[] RawRequest => (byte[])base.RawRequest;
        public new RequestType CommandName
        {
            get => (RequestType)base.CommandName;
            protected set => base.CommandName = value;
        }
        public RequestBase(object rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            RequestLength = ByteTools.ToUInt16(ByteTools.SubBytes(RawRequest, 0, 2), true);
        }
    }
}
