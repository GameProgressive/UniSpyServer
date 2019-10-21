using GameSpyLib.Common;
using PresenceConnectionManager.Enumerator;
using System;
using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.Buddy.AddBuddy
{
    public class AddBuddyHandler
    {
        
        static Dictionary<string, string> _recv;
        static GPErrorCode _errorCode;
       
        public static void Addfriends(GPCMSession session, Dictionary<string, string> recv)
        {
            _recv = recv;
            _errorCode = GPErrorCode.NoError;
           
            //\addbuddy\\sesskey\<>\newprofileid\<>\reason\<>\final\
            IsContainAllKey();
            if (_errorCode != GPErrorCode.NoError)
            {
                return;
            }
            if (session.PlayerInfo.Profileid == Convert.ToUInt16(_recv["profileid"]))
            {
                //you can not add yourself friend
                return;
            }
            AddBuddyQuery.SaveAddBuddyRequest(session.PlayerInfo.Profileid, Convert.ToUInt16(_recv["newprofileid"]), session.PlayerInfo.Namespaceid, _recv["reason"]);

        }

        private static void IsContainAllKey()
        {
            if (!_recv.ContainsKey("sesskey") || !_recv.ContainsKey("newprofileid") || !_recv.ContainsKey("reason"))
            {
                _errorCode = GPErrorCode.Parse;
            }
        }
    }
}
