using System;
using System.Collections.Generic;
using PresenceConnectionManager.Enumerator;

namespace PresenceConnectionManager.Handler.Buddy.AddBlock
{
    public class AddBlockHandler
    {
        static Dictionary<string, string> _recv;
        static GPErrorCode _errorCode;
        public static void AddUserToBlockList(GPCMSession session, Dictionary<string, string> recv)
        {
            _recv = recv;
            _errorCode = GPErrorCode.NoError;

            IsContainAllKey();
            if (_errorCode != GPErrorCode.NoError)
            { return; }
            AddBlockQuery.UpdateBlockListInDatabase(session.PlayerInfo.Profileid, Convert.ToUInt16(_recv["profileid"]),session.PlayerInfo.Namespaceid);

        }

        private static void IsContainAllKey()
        {
            if (!_recv.ContainsKey("profileid"))
            {
                _errorCode = GPErrorCode.Parse;
            }
        }
    }
}
