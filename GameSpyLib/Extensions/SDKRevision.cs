namespace PresenceConnectionManager.Structure
{
    public static class SDKRevision
    {
        public const int GPINewAuthNotification = 1;
        public const int GPINewRevokeNotification = 2;
        public const int GPINewStatusNotification = 4;
        public const int GPINewListRetrevalOnLogin = 8;
        public const int GPIRemoteAuthIDSNotification = 16;
        public const int GPINewCDKeyRegistration = 32;

        public const int Crysis2SDKRevision =
            SDKRevisionType3;


        public const int SDKRevisionType1 =
            GPINewAuthNotification +
            GPINewRevokeNotification +
            GPINewListRetrevalOnLogin;

        public const int SDKRevisionType2 =
            GPINewAuthNotification +
            GPINewRevokeNotification +
            GPINewStatusNotification +
            GPINewListRetrevalOnLogin;

        public const int SDKRevisionType3 =
            GPINewAuthNotification +
            GPINewRevokeNotification +
            GPINewListRetrevalOnLogin +
            GPIRemoteAuthIDSNotification +
            GPINewCDKeyRegistration;
    }
}
