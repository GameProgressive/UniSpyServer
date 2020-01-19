using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceConnectionManager.Handler.General.RegisterCDKey
{
    public class RegisterCDKeyHandler : GPCMHandlerBase
    {
        public RegisterCDKeyHandler(Dictionary<string, string> recv) : base(recv)
        {
            _sendingBuffer = @"\rc\final\";
        }

    }
}
