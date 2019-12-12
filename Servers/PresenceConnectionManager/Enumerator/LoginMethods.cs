using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceConnectionManager.Enumerator
{
    /// <summary>
    /// This enumation defins the supported login method for the users.
    /// </summary>
    public enum LoginMethods
    {
        /// <summary>
        /// Login with user combo (nick@email)
        /// </summary>
        Username = 0,

        /// <summary>
        /// Login with unique nickname
        /// </summary>
        UniqueNickname,

        /// <summary>
        /// Pre-authenticated login
        /// </summary>
        AuthToken
    }
}
