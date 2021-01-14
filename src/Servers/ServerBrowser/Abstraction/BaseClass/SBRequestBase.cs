using UniSpyLib.Abstraction.BaseClass;

namespace ServerBrowser.Abstraction.BaseClass
{
    public class SBRequestBase : UniSpyRequestBase
    {
        public new bool ErrorCode
        {
            get { return (bool)base.ErrorCode; }
            protected set { base.ErrorCode = value; }
        }
        public SBRequestBase(object rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            ErrorCode = true;
        }
    }
}
