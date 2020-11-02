using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Enumerate;
using System;
using System.Collections.Generic;

namespace QueryReport.Entity.Structure.Request
{
    public class AddErrorRequest : QRRequestBase
    {
        public AddErrorRequest(byte[] rawRequest) : base(rawRequest)
        {
            throw new NotImplementedException();
        }
    }
}
