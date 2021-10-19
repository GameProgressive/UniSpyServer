
namespace PresenceConnectionManager.Entity.Enumerate
{
    public enum SDKRevisionType : uint
    {
        Unknown = 0,
        /// <summary>
        /// Extended message support
        /// </summary>
        GPINewAuthNotification = 1,//1
        /// <summary>
        /// Remove friend from remote
        /// </summary>
        GPINewRevokeNotification = 1 << 2,//10
        /// <summary>
        /// New Status Info support
        /// </summary>
        GPINewStatusNotification = 1 << 3,//1000
        /// <summary>
        /// Buddy List + Block List retrieval on login
        /// </summary>
        GPINewListRetrevalOnLogin = 1 << 4,//
        /// <summary>
        /// Remote Auth logins now return namespaceid/partnerid on login
        /// </summary>
        GPIRemoteAuthIDSNotification = 1 << 5,
        /// <summary>
        /// New CD Key registration style as opposed to using product ids
        /// </summary>
        GPINewCDKeyRegistration = 1 << 6,

    }
}
