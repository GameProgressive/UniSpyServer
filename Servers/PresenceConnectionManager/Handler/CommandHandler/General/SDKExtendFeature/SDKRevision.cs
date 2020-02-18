using System;
using System.Collections;

namespace PresenceConnectionManager.Handler.General.SDKExtendFeature
{
    public static class SDKRevision
    {
        /// <summary>
        /// Extended message support
        /// </summary>
        public const uint GPINewAuthNotification = 0b_1;//1

        /// <summary>
        /// Remove friend from remote
        /// </summary>
        public const uint GPINewRevokeNotification = 0b_10;//10
        /// <summary>
        /// New Status Info support
        /// </summary>
        public const uint GPINewStatusNotification = 0b_100;//1000
        /// <summary>
        /// Buddy List + Block List retrieval on login
        /// </summary>
        public const uint GPINewListRetrevalOnLogin = 0b_1000;//
        /// <summary>
        /// Remote Auth logins now return namespaceid/partnerid on login
        /// </summary>
        public const uint GPIRemoteAuthIDSNotification = 0b_10000;
        /// <summary>
        /// New CD Key registration style as opposed to using product ids
        /// </summary>
        public const uint GPINewCDKeyRegistration = 0b_100000;

        /// <summary>
        /// Tell server send back extra information according to the number of  sdkrevision
        /// </summary>
        public static void ExtendedFunction(GPCMSession session)
        {

            var bits = new BitArray(BitConverter.GetBytes(session.UserInfo.SDKRevision));
            if (bits[0])
            {
                //Send add friend request
            }
            if (bits[1])
            {
                //send revoke request
            }
            if (bits[2])
            {
                //send new status info
            }
            if (bits[3])
            {
                //send buddy list and block list
            }
            if (bits[4])
            {
                //Remote auth
            }
            if (bits[5])
            {
                //register cdkey with product id
            }

        }
    }
}
