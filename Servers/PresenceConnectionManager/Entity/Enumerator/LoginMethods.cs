using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceConnectionManager.Enumerator
{
    /// <summary>
    /// This enumation defins the supported login method for the users.
    /// </summary>
    public enum LoginType:uint
    {

        NotFound,
        /// <summary>
        /// Login with user combo (nick@email)
        /// </summary>
        Nick,

        /// <summary>
        /// Login with unique nickname
        /// </summary>
        Uniquenick,

        /// <summary>
        /// Pre-authenticated login
        /// </summary>
        AuthToken
    }
}
