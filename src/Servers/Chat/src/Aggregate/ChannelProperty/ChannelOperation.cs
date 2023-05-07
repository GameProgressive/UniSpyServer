using System.Linq;
using UniSpy.Server.QueryReport.Aggregate.Redis.Channel;

namespace UniSpy.Server.Chat.Aggregate
{
    public partial class Channel
    {
        public ChannelInfo GetChannelCache()
        {
            var chanInfo = new ChannelInfo()
            {
                ServerId = this.ServerId,
                Name = this.Name,
                MaxNumberUser = this.MaxNumberUser,
                CreateTime = this.CreateTime,
                RoomType = this.RoomType,
                GameName = this.GameName,
                PreviousJoinedChannel = this.PreviousJoinedChannel,
                Users = this.Users.Where(u => u.Value.Info.IsRemoteClient == false).Select(u => u.Key).ToList()
            };
            return chanInfo;
        }
        public static void UpdateChannelCache(ChannelUser user)
        {
            if (user.Client.Info.IsRemoteClient)
            {
                return;
            }
            var channelInfo = user.Channel.GetChannelCache();
            if (!QueryReport.Application.StorageOperation.UpdateChannel(channelInfo))
            {
                throw new Error.IRC.Channel.NoSuchChannelException("Update channel on redis fail.", user.Channel.Name);
            }
        }
    }
}