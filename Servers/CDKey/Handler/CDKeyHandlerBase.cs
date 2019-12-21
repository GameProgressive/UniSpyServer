using GameSpyLib.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CDKey.Handler
{
    public class CDKeyHandlerBase : HandlerBase<CDKeyServer, Dictionary<string, string>>
    {
        Dictionary<string, string> _recv;
        public CDKeyHandlerBase(Dictionary<string,string> recv)
        {
            _recv = recv;
        }
    }
}
