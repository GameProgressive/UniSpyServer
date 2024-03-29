using UniSpy.Server.Chat.Aggregate.Redis.Contract;
using UniSpy.Server.Chat.Handler;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Extension.Redis;
using System.Threading.Tasks;
using UniSpy.Server.Chat.Application;
using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Core.Encryption;

namespace UniSpy.Server.Chat.Aggregate.Redis
{
    /// <summary>
    /// The general chat message will process here
    /// </summary>
    public class GeneralMessageChannel : RedisChannelBase<RemoteMessage>
    {
        public GeneralMessageChannel() : base(RedisChannelName.ChatChannelPrefix)
        {
        }
        public override void ReceivedMessage(RemoteMessage message)
        {
            if (message.Client.Server.Id == ServerLauncher.Server.Id)
            {
                return;
            }
            if (message.Type == "DISCONNECT")
            {
                ClientManager.RemoveClient(message.Client);
                return;
            }
            IChatClient client = (IChatClient)ClientManager.GetClient(message.Client);
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

            var switcher = new CmdSwitcher(client, UniSpyEncoding.GetString(message.RawRequest));
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
    /// <summary>
    /// When a local channel is created the user message will send to redis channel
    /// redis channel is like a broadcast platform which will broadcast the message to all the user
    /// when user is connected to unispy chat server
    /// </summary>
    public class ChatMessageChannel : RedisChannelBase<RemoteMessage>
    {
        public ChatMessageChannel(string chatChannelName) : base($"{RedisChannelName.ChatChannelPrefix}:{chatChannelName}") { }

        public override void ReceivedMessage(RemoteMessage message)
        {
            // base.ReceivedMessage(message);
            if (message.Client.Server.Id == ServerLauncher.Server.Id)
            {
                return;
            }
            IChatClient client = (IChatClient)ClientManager.GetClient(message.Client);
            if (client is null)
            {
                throw new Chat.Exception($"There are no remote client found in RemoteClients pool, the client must be login on the remote server.");
            }

            var switcher = new CmdSwitcher(client, UniSpyEncoding.GetString(message.RawRequest));
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
