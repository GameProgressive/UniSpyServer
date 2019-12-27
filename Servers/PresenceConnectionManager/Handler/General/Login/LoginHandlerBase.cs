using System;
using System.Collections.Generic;
using GameSpyLib.Extensions;

namespace PresenceConnectionManager.Handler.HandlerBase
{
    public class LoginHandlerBase : GPCMHandlerBase
    {
        protected Crc16 _crc = new Crc16(Crc16Mode.Standard);

        protected string _errorMsg;

        public LoginHandlerBase(Dictionary<string, string> recv) : base(recv)
        { 
        }

    }
}
