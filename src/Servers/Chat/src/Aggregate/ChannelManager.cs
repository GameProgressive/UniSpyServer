using System.Collections.Concurrent;
using UniSpy.Server.Chat.Abstraction.Interface;

namespace UniSpy.Server.Chat.Aggregate
{
    public static class ChannelManager
    {
        public static readonly ConcurrentDictionary<string, Channel> Channels = new ConcurrentDictionary<string, Channel>();
        public static bool IsChannelExist(string name)
        {
            return Channels.ContainsKey(name);
        }
        /// <summary>
        /// You need to manually check channel existance then get channel
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Channel GetChannel(string name)
        {
            if (!Channels.TryGetValue(name, out var channel))
            {
                throw new Chat.Exception("Channel do not exist!");
            }
            return channel;
        }
        public static void RemoveChannel(string name)
        {
            Channels.TryRemove(name, out var chan);
            Application.StorageOperation.Persistance.RemoveChannel(chan);
        }
        public static Channel CreateChannel(string name, string password = null, IChatClient creator = null)
        {
            var channel = new Channel(name, creator, password);

            if (!Channels.TryAdd(name, channel))
            {
                Channels.TryGetValue(name, out channel);
            }
            return channel;
        }
    }
}