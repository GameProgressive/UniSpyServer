using System.Collections.Generic;
using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Chat.Application;
using UniSpy.Server.Chat.Exception;

namespace UniSpy.Server.Chat.Aggregate.Misc.ChannelInfo
{
    public static class ChannelManager
    {
        public static readonly Dictionary<string, Channel> Channels = new Dictionary<string, Channel>();
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
            if (!IsChannelExist(name))
            {
                throw new ChatException("Channel do not exist!");
            }
            return Channels[name];
        }
        public static void RemoveChannel(string name)
        {
            if (!IsChannelExist(name))
            {
                throw new ChatException("Channel do not exist!");
            }
            lock (Channels)
            {
                Channels.Remove(name);
            }
        }
        public static Channel CreateChannel(string name, string password = null, IChatClient creator = null)
        {
            
            var channel = new Channel(name, creator, password);

            lock (Channels)
            {
                Channels.Add(channel.Name, channel);
            }
            return channel;
        }
    }
}