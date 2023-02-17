using UniSpy.Server.PresenceConnectionManager.Entity.Enumerate;

namespace UniSpy.Server.PresenceConnectionManager.Entity.Structure.Misc
{
    public sealed class SdkRevision
    {
        public SdkRevisionType? SDKRevisionType { get; set; }
        public bool IsSDKRevisionValid => SDKRevisionType == 0 ? false : true;
        public bool IsSupportGPINewAuthNotification => (SDKRevisionType ^ Enumerate.SdkRevisionType.GPINewAuthNotification) != 0 ? true : false;
        public bool IsSupportGPINewRevokeNotification => (SDKRevisionType ^ Enumerate.SdkRevisionType.GPINewRevokeNotification) != 0 ? true : false;
        public bool IsSupportGPINewStatusNotification => (SDKRevisionType ^ Enumerate.SdkRevisionType.GPINewStatusNotification) != 0 ? true : false;
        public bool IsSupportGPINewListRetrevalOnLogin => (SDKRevisionType ^ Enumerate.SdkRevisionType.GPINewListRetrevalOnLogin) != 0 ? true : false;
        public bool IsSupportGPIRemoteAuthIDSNotification => (SDKRevisionType ^ Enumerate.SdkRevisionType.GPIRemoteAuthIDSNotification) != 0 ? true : false;
        public bool IsSupportGPINewCDKeyRegistration => (SDKRevisionType ^ Enumerate.SdkRevisionType.GPINewCDKeyRegistration) != 0 ? true : false;
        public SdkRevision()
        {
        }
    }
}
