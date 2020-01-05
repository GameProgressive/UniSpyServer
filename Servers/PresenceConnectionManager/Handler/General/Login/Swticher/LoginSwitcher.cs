using System;
using System.Collections.Generic;
using PresenceConnectionManager.Enumerator;
using PresenceConnectionManager.Handler.General.Login.LoginMethod;

namespace PresenceConnectionManager.Handler.General.Login
{
    public class LoginSwitcher
    {
        public static void Switch(GPCMSession session, Dictionary<string, string> recv)
        {
            if (recv.ContainsKey("uniquenick"))
            {
                session.PlayerInfo.LoginType = LoginType.Uniquenick;
                UniquenickLogin uniquenickLogin = new UniquenickLogin(recv);
                uniquenickLogin.Handle(session);
            }
            else if (recv.ContainsKey("authtoken"))
            {
                session.PlayerInfo.LoginType = LoginType.AuthToken;
                AuthtokenLogin authtoken = new AuthtokenLogin(recv);
                authtoken.Handle(session);
            }
           else if (recv.ContainsKey("nick"))
            {
                session.PlayerInfo.LoginType = LoginType.Nick;
                NickEmailLogin nickEmailLogin = new NickEmailLogin(recv);
                nickEmailLogin.Handle(session);
            }
            else
            {
                //if no login method found we can not continue.
                session.ToLog("Unknown login method detected!");
                return;
            }
        }

    }
}
