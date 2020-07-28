using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Logging;
using PresenceConnectionManager.Entity.Enumerator;
using PresenceConnectionManager.Entity.Structure;

namespace PresenceConnectionManager.Entity.Structure
{
    public static class SDKRevision
    {
        /// <summary>
        /// Tell server send back extra information according to the number of  sdkrevision
        /// </summary>
        public static void ExtendedFunction(ISession client)
        {
            PCMSession _session = (PCMSession)client.GetInstance();
            if (_session.UserData.SDKRevision == 0)
            {
                LogWriter.ToLog(Serilog.Events.LogEventLevel.Error, "[SDKRev] No sdkrevision!");
                return;
            }

            if ((_session.UserData.SDKRevision ^ SDKRevisionType.GPINewAuthNotification) != 0)
            {
                //Send add friend request
            }

            if ((_session.UserData.SDKRevision ^ SDKRevisionType.GPINewRevokeNotification) != 0)
            {
                //send revoke request
            }

            if ((_session.UserData.SDKRevision ^ SDKRevisionType.GPINewStatusNotification) != 0)
            {
                //send new status info
            }

            if ((_session.UserData.SDKRevision ^ SDKRevisionType.GPINewListRetrevalOnLogin) != 0)
            {
                //send buddy list and block list
            }

            if ((_session.UserData.SDKRevision ^ SDKRevisionType.GPIRemoteAuthIDSNotification) != 0)
            {
                //Remote auth
            }

            if ((_session.UserData.SDKRevision ^ SDKRevisionType.GPINewCDKeyRegistration) != 0)
            {
                //register cdkey with product id
            }
        }
    }
}
