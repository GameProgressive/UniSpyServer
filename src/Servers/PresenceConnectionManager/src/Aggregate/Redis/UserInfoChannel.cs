using System;
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
            // when we are in debug mode, the linq operation will not be executed, so we have to execute manually to make program do not crash
            if (System.Diagnostics.Debugger.IsAttached)
            {
                _ = ((ClientInfo)(message.Client.Info)).UserInfo;
                _ = ((ClientInfo)(message.Client.Info)).ProfileInfo;
                _ = ((ClientInfo)(message.Client.Info)).SubProfileInfo;
            }

            if (message.Client.Server.Id == ServerLauncher.Server.Id)
            {
                return;
            }
            var client = (IShareClient)ClientManager.GetClient(message.Client.Connection.RemoteIPEndPoint);
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