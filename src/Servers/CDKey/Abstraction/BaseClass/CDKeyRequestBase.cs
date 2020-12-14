using System;
using System.Collections.Generic;
using System.Text;
using UniSpyLib.Abstraction.BaseClass;

namespace CDKey.Abstraction.BaseClass
{
    public class CDKeyRequestBase : UniSpyRequestBase
    {
        public new bool ErrorCode { get { return (bool)base.ErrorCode; } set { base.ErrorCode = value; } }
        public CDKeyRequestBase(string rawRequest) : base(rawRequest)
        {
            ErrorCode = false;
        }

        public override void Parse()
        {
            ErrorCode = true;
        }
    }
}
