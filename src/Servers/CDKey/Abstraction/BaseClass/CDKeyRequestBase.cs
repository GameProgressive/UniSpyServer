using System;
using System.Collections.Generic;
using System.Text;
using UniSpyLib.Abstraction.BaseClass;

namespace CDKey.Abstraction.BaseClass
{
    public class CDKeyRequestBase : RequestBase
    {
        public CDKeyRequestBase(string rawRequest) : base(rawRequest)
        {
        }

        public override object Parse()
        {
            return true;
        }
    }
}
