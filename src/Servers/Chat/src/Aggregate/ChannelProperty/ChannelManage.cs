using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Chat.Aggregate.Redis;

namespace UniSpy.Server.Chat.Aggregate
{
    /// <summary>
    /// The code manage local and remote channel
    /// </summary>
    public sealed partial class Channel
    {
        /// <summary>
        /// The local channel manager
        /// </summary>
        [JsonIgnore]
        public static readonly ConcurrentDictionary<string, Channel> LocalChannels = new();
        public static readonly ConcurrentDictionary<string, ChannelMessageBroker> MessageBrokers = new();
        /// <summary>
        /// You need to manually check channel existance then get channel
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Channel GetLocalChannel(string name)
        {
            LocalChannels.TryGetValue(name, out var channel);
            return channel;
        }
        public static void RemoveLocalChannel(Channel channel)
        {
            LocalChannels.TryRemove(channel.Name, out var chan);
            RemoveMessageBrocker(channel);
        }
        public static Channel CreateLocalChannel(string name, IShareClient creator = null, string password = null)
        {
            var channel = new Channel(name, creator, password);
            LocalChannels.TryAdd(name, channel);
            AddMessageBrocker(channel);
            return channel;
        }
        public static void UpdateChannelCache(ChannelUser user, Channel channel)
        {
            if (user.Client.IsRemoteClient)
            {
                return;
            }
            Application.StorageOperation.Persistance.UpdateChannel(channel);
        }
        public static Channel GetChannelCache(ChannelCache key)
        {
            return Application.StorageOperation.Persistance.GetChannel(key);
        }
        public static void RemoveChannelCache(ChannelUser user, Channel channel)
        {
            if (user.Client.IsRemoteClient)
            {
                return;
            }
            StackExchange.Redis.RedisValue token = Environment.MachineName;
            if (Application.StorageOperation.Persistance.ChannelCacheClient.Db.LockTake(channel.Name, token, TimeSpan.FromSeconds(10)))
            {
                Application.StorageOperation.Persistance.RemoveChannel(channel);
            }
        }

        public static ChannelMessageBroker AddMessageBrocker(Channel channel)
        {
            ChannelMessageBroker broker;
            if (!MessageBrokers.TryGetValue(channel.Name, out broker))
            {
                broker = new ChannelMessageBroker(channel.Name);
                broker.Subscribe();
                MessageBrokers.TryAdd(channel.Name, broker);
            }
            return broker;
        }
        public static void RemoveMessageBrocker(Channel channel)
        {
            MessageBrokers.TryRemove(channel.Name, out var _);
        }
    }
}