using System;
using System.Collections.Generic;
using PresenceConnectionManager.Enumerator;

namespace PresenceConnectionManager.Handler.General.Login.LoginMethod
{
    public class AuthtokenLogin:LoginHandlerBase
    {
        public AuthtokenLogin(Dictionary<string, string> recv) : base(recv)
        {
        }

        protected override void CheckRequest(GPCMSession session)
        {
            base.CheckRequest(session);

            //If do not pass the base.CheckRequest we can not continue.
            if (_errorCode != GPErrorCode.NoError)
            {
                return;
            }
            session.PlayerInfo.AuthToken = _recv["authtoken"];
        }

        protected override void DataBaseOperation(GPCMSession session)
        {
            //TODO
            _result = null;
            //base.DataBaseOperation(session);
        }
    }
}
