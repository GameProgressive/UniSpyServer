using System;
using System.Collections.Generic;
using System.Text;
using PresenceConnectionManager.DatabaseQuery;
using PresenceConnectionManager.Structure;

namespace PresenceConnectionManager.Handler.LoginMethod
{
    public class NoUniquenickLogin
    {
        public static void Login()
        {
            LoginHandler.ProcessNickAndEmail();
            //LoginHandler.ProcessPassword();

            if (!LoginHandler.Recv.ContainsKey("namespaceid"))
            {
                LoginHandler.Recv.Add("namespaceid", "0");
            }
            LoginHandler.QueryResult = LoginQuery.GetUserFromNickAndEmail(LoginHandler.Recv);
            LoginHandler.Session.PlayerInfo.Profileid = Convert.ToUInt32(LoginHandler.QueryResult["profileid"]);
            LoginHandler.SendLoginResponseChallenge();

        }

       
    }

}
