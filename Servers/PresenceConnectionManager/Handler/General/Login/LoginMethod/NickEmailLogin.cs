using System;
using System.Collections.Generic;
using PresenceConnectionManager.Enumerator;

namespace PresenceConnectionManager.Handler.General.Login.LoginMethod
{
    public class NickEmailLogin : LoginHandlerBase
    {
        public NickEmailLogin(Dictionary<string, string> recv) : base(recv)
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

            // "User" is <nickname>@<email>, it will be splitted in this function

            string user = _recv["user"];

            int Pos = user.IndexOf('@');
            if (Pos == -1 || Pos < 1 || (Pos + 1) >= user.Length)
            {
                // Ignore malformed user
                // Pos == -1 : Not found
                // Pos < 1 : @ or @example
                // Pos + 1 >= Length : example@
                _errorCode = GPErrorCode.LoginBadEmail;
                return;
            }

            string nick = user.Substring(0, Pos);
            string email = user.Substring(Pos + 1);

            session.PlayerInfo.Nick = nick;
            session.PlayerInfo.Email = email;
            session.PlayerInfo.LoginType = LoginType.Nick;
        }

        protected override void DataBaseOperation(GPCMSession session)
        {
            
            _result = LoginQuery.GetUserFromNickAndEmail(session.PlayerInfo.NamespaceID, session.PlayerInfo.Nick, session.PlayerInfo.Email);
            base.DataBaseOperation(session);
        }
    }
}
