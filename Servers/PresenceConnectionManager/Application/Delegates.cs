using PresenceConnectionManager;


namespace PresenceConnectionManager.Application
{
    public delegate void GPCMConnectionUpdate(GPCMClient client);
    public delegate void GPCMConnectionClosed(GPCMClient client);
    public delegate void GPCMStatusChanged(GPCMClient client);

}
