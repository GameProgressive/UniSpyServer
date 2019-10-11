using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceConnectionManager.Structure
{
    public static class GameSpySDKRevision
    {
        /// <summary>
        /// Extended message support
        /// </summary>
        public const int GPINewAuthNotification = 1;
        public const int GPINewRevokeNotification = 2;
        /// <summary>
        /// New Status Info support
        /// </summary>
        public const int GPINewStatusNotification = 4;
        /// <summary>
        /// Buddy List + Block List retrieval on login
        /// </summary>
        public const int GPINewListRetrevalOnLogin = 8;
        /// <summary>
        /// Remote Auth logins now return namespaceid/partnerid on login
        /// </summary>
        public const int GPIRemoteAuthIDSNotification = 16;
        /// <summary>
        /// New CD Key registration style as opposed to using product ids
        /// </summary>
        public const int GPINewCDKeyRegistration = 32;

        public const int Crysis2SDKRevision =
            Type3;


        public const int Type1 = 
            GPINewAuthNotification +
            GPINewRevokeNotification +
            GPINewListRetrevalOnLogin;

        public const int Type2 = 
            GPINewAuthNotification + 
            GPINewRevokeNotification + 
            GPINewStatusNotification + 
            GPINewListRetrevalOnLogin;

        public const int Type3 =
            GPINewAuthNotification +
            GPINewRevokeNotification +
            GPINewListRetrevalOnLogin +
            GPINewCDKeyRegistration;

        public const int Type4 =
            GPINewAuthNotification +
            GPINewRevokeNotification +
            GPINewListRetrevalOnLogin +
            GPIRemoteAuthIDSNotification +
            GPINewCDKeyRegistration;
        
    }
}
