using System;
using System.Collections.Generic;

namespace GameSpyLib.Extensions
{
    public class RequestCheck
    {
        public static bool IsUniqueNickLegal(string uniquenick)
        {
            if (uniquenick.Length > 3 && uniquenick.Length < 15)
            {
                return true;
            }

            return false;
        }
        public static bool IsNicknameLegal(string nick)
        {
            if (nick.Length > 2 && nick.Length < 15)
            {
                return true;
            }

            return false;
        }
        public static bool IsEmailLegal(string email)
        {
            return true;

        }
        public static bool IsNamespaceidLegal(ushort namespaceid)
        {
            return true;
        }
        public static bool IsPartneridLegal(ushort partnerid)
        {
            return true;
        }

        public static bool IsPasswordLegal(string password)
        {
            if (password.Length > 4 && password.Length < 20)
            {
                return true;
            }
            return false;
        }
    }
}
