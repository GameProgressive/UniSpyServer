using System;
using System.Collections.Generic;
using PresenceConnectionManager.Enumerator;

namespace PresenceConnectionManager.Handler.General.Login.LoginMethod
{
    public class UniquenickLogin : LoginHandlerBase
    {
        public UniquenickLogin(Dictionary<string, string> recv) : base(recv)
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
            session.PlayerInfo.UniqueNick = _recv["uniquenick"];

        }

        protected override void DataBaseOperation(GPCMSession session)
        {
            _result = LoginQuery.GetUserFromUniqueNick(session.PlayerInfo.UniqueNick, session.PlayerInfo.NamespaceID);
            base.DataBaseOperation(session);
        }
    }
}
