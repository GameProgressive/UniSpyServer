// using System.Collections.Concurrent;
// using System.Collections.Generic;
// using UniSpyServer.Servers.Chat.Entity.Structure.Misc.ChannelInfo;

// namespace UniSpyServer.Servers.Chat.Handler.SystemHandler.ChannelManage
// {
//     public sealed class ChannelManager
//     {
//         public static IDictionary<string, Channel> Channels { get; private set; }
//         static ChannelManager()
//         {
//             Channels = new ConcurrentDictionary<string, Channel>();
//         }
//         public void Start()
//         {
//             //start timer to check expired channel
//         }
//         public static bool GetChannel(string name, out Channel channel)
//         {
//             return Channels.TryGetValue(name, out channel);
//         }
//         public static bool AddChannel(string name, Channel channel)
//         {
//             return Channels.TryAdd(name, channel);
//         }
//         public static bool RemoveChannel(string name)
//         {
//             return Channels.Remove(name, out _);
//         }
//         public static bool RemoveChannel(Channel channel)
//         {
//             return RemoveChannel(channel.Name);
//         }
//     }
// }
