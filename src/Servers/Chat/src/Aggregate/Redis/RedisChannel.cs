using System.Collections.Concurrent;
using System.Net;
using UniSpy.Server.Chat.Aggregate.Redis.Contract;
using UniSpy.Server.Chat.Exception;
using UniSpy.Server.Chat.Handler;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Extension.Redis;
using System.Threading.Tasks;
using UniSpy.Server.Chat.Application;

namespace UniSpy.Server.Chat.Aggregate.Redis
{
    /// <summary>
    /// The general chat message will process here
    /// </summary>
    public class GeneralMessageChannel : RedisChannelBase<RemoteMessage>
    {
        public static readonly ConcurrentDictionary<IPEndPoint, IClient> RemoteClients = new ConcurrentDictionary<IPEndPoint, IClient>();
        public GeneralMessageChannel() : base(RedisChannelName.ChatChannelPrefix)
        {
        }
        public override void ReceivedMessage(RemoteMessage message)
        {
            if (message.Client.Connection.Server.ServerID == ServerLauncher.ServerInstance.ServerID)
            {
                return;
            }
            if (message.Type == "DISCONNECT")
            {
                RemoteClients.TryRemove(message.Client.Connection.RemoteIPEndPoint, out _);
                return;
            }
            IClient client;
            if (!RemoteClients.TryGetValue(message.Client.Connection.RemoteIPEndPoint, out client))
            {
                RemoteClients.TryAdd(message.Client.Connection.RemoteIPEndPoint, message.Client);
                client = message.Client;
            }

            var switcher = new CmdSwitcher(client, message.RawRequest);
            if (System.Diagnostics.Debugger.IsAttached)
            {
                switcher.Switch();
            }
            else
            {
                Task.Run(() => switcher.Switch());
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
            if (message.Client.Connection.Server.ServerID == ServerLauncher.ServerInstance.ServerID)
            {
                return;
            }
            IClient client;
            if (!GeneralMessageChannel.RemoteClients.TryGetValue(message.Client.Connection.RemoteIPEndPoint, out client))
            {
                throw new ChatException($"There are no remote client found in RemoteClients pool, the client must be login on the remote server.");
            }

            var switcher = new CmdSwitcher(client, message.RawRequest);
            if (System.Diagnostics.Debugger.IsAttached)
            {
                switcher.Switch();
            }
            else
            {
                Task.Run(() => switcher.Switch());
            }
        }
    }
}
