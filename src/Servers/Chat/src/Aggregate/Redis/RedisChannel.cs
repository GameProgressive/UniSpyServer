using UniSpy.Server.Chat.Aggregate.Redis.Contract;
using UniSpy.Server.Chat.Handler;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Extension.Redis;
using System.Threading.Tasks;
using UniSpy.Server.Chat.Application;
using UniSpy.Server.Core.Encryption;

namespace UniSpy.Server.Chat.Aggregate.Redis
{
    /// <summary>
    /// When a local channel is created the user message will send to redis channel
    /// redis channel is like a broadcast platform which will broadcast the message to all the user
    /// when user is connected to unispy chat server
    /// </summary>
    public class ChannelMessageBroker : RedisChannelBase<RemoteMessage>
    {
        public ChannelMessageBroker(string chatChannelName) : base($"{RedisChannelName.ChatChannelPrefix}:{chatChannelName}") { }

        public override void ReceivedMessage(RemoteMessage message)
        {
            // we are uint testing
            if (ServerLauncher.Server is null)
            {
                return;
            }
            // base.ReceivedMessage(message);
            if (message.Client.Server.Id == ServerLauncher.Server.Id)
            {
                return;
            }
            var switcher = new CmdSwitcher(message.Client, UniSpyEncoding.GetString(message.RawRequest));
            if (System.Diagnostics.Debugger.IsAttached)
            {
                switcher.Handle();
            }
            else
            {
                Task.Run(() => switcher.Handle());
            }
        }
    }
}
