using UniSpyServer.Servers.PresenceConnectionManager.Entity.Enumerate;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Misc
{
    public sealed class PCMSDKRevision
    {
        public SDKRevisionType? SDKRevisionType { get; set; }
        public bool IsSDKRevisionValid => SDKRevisionType == 0 ? false : true;
        public bool IsSupportGPINewAuthNotification => (SDKRevisionType ^ Enumerate.SDKRevisionType.GPINewAuthNotification) != 0 ? true : false;
        public bool IsSupportGPINewRevokeNotification => (SDKRevisionType ^ Enumerate.SDKRevisionType.GPINewRevokeNotification) != 0 ? true : false;
        public bool IsSupportGPINewStatusNotification => (SDKRevisionType ^ Enumerate.SDKRevisionType.GPINewStatusNotification) != 0 ? true : false;
        public bool IsSupportGPINewListRetrevalOnLogin => (SDKRevisionType ^ Enumerate.SDKRevisionType.GPINewListRetrevalOnLogin) != 0 ? true : false;
        public bool IsSupportGPIRemoteAuthIDSNotification => (SDKRevisionType ^ Enumerate.SDKRevisionType.GPIRemoteAuthIDSNotification) != 0 ? true : false;
        public bool IsSupportGPINewCDKeyRegistration => (SDKRevisionType ^ Enumerate.SDKRevisionType.GPINewCDKeyRegistration) != 0 ? true : false;
        public PCMSDKRevision()
        {
        }
    }
}
