using PresenceConnectionManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceConnectionManager.Handler
{
    public class InviteToHandler
    {
        //public static GPCMDBQuery DBQuery = null;
        public static void AddProducts(GPCMClient client, Dictionary<string, string> recv)
        {
            ushort readedSessionKey = 0;

            if (!recv.ContainsKey("products") || !recv.ContainsKey("sesskey"))
                return;

            if (!ushort.TryParse(recv["sesskey"], out readedSessionKey))
                return;

            if (readedSessionKey != client.SessionKey || readedSessionKey == 0)
                return;
        }
    }
}
