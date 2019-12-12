using System;
using System.Collections.Generic;
using System.Text;

namespace GameSpyLib.Common
{
    public abstract class HandlerBase<Source>
    {
        public abstract void Handle(Source source);

        protected abstract void CheckRequest();

    }
}
