using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Extension.Redis;
using UniSpy.Server.PresenceConnectionManager.Abstraction.Interface;
using UniSpy.Server.PresenceConnectionManager.Application;

namespace UniSpy.Server.PresenceConnectionManager.Aggregate.Redis
{
    public class UserInfoChannel : RedisChannelBase<UserInfoMessage>
    {
        public UserInfoChannel() : base(RedisChannelName.PresenceConnectionManagerUserInfoChannel)
        {
        }
        public override void ReceivedMessage(UserInfoMessage message)
        {
            // base.ReceivedMessage(message);
            if (message.Client.Server.Id == ServerLauncher.Server.Id)
            {
                return;
            }

            IShareClient client = (IShareClient)ClientManager.GetClient(message.Client.Connection.RemoteIPEndPoint);
            if (client is null)
            {
                ClientManager.AddClient(message.Client);
                client = message.Client;
            }
            else
            {
                // we update the remote client info
                ((RemoteClient)client).Info = message.Client.Info;
            }
        }
    }
}