using ServerBrowser.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Extensions;

namespace ServerBrowser.Abstraction.BaseClass
{
    internal abstract class SBRequestBase : UniSpyRequestBase
    {
        public new SBErrorCode ErrorCode
        {
            get { return (SBErrorCode)base.ErrorCode; }
            protected set { base.ErrorCode = value; }
        }

        public new SBClientRequestType CommandName
        {
            get { return (SBClientRequestType)base.CommandName; }
            protected set { base.CommandName = value; }
        }
        public int RequestLength { get; private set; }
        public new byte[] RawRequest => (byte[])base.RawRequest;
        public SBRequestBase(object rawRequest) : base(rawRequest)
        {
            ErrorCode = SBErrorCode.NoError;
        }

        public override void Parse()
        {
            RequestLength = ByteTools.ToUInt16(ByteTools.SubBytes(RawRequest, 0, 2), true);
        }
    }
}
