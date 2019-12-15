using System;
using System.Collections.Generic;
using GameSpyLib.Extensions;

namespace PresenceConnectionManager.Handler.HandlerBase
{
    public abstract class LoginHandlerBase : GPCMHandlerBase
    {
        protected Crc16 _crc = new Crc16(Crc16Mode.Standard);

        protected string _errorMsg;

        public LoginHandlerBase(GPCMSession session,Dictionary<string, string> recv) : base(recv)
        {
            session.PlayerInfo.DisconReason = Enumerator.DisconnectReason.NormalLogout;
        }

        public abstract void Handle(GPCMSession session);

        protected abstract void CheckRequest(GPCMSession session);
    }
}
