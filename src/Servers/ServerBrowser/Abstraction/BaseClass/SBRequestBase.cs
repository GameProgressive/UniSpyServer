using System;
using UniSpyLib.Abstraction.BaseClass;

namespace ServerBrowser.Abstraction.BaseClass
{
    public class SBRequestBase : RequestBase
    {
        public SBRequestBase(object rawRequest) : base(rawRequest)
        {
        }

        public override object Parse()
        {
            return true;
        }
    }
}
