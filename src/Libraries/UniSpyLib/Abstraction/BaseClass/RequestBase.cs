using System;
using System.Collections.Generic;
using System.Text;
using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class RequestBase
    {
        public RequestBase()
        {
            LogWriter.LogCurrentClass(this);
        }
    }
}
