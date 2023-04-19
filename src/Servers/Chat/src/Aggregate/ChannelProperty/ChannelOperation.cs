using System;
using System.Linq;


namespace UniSpy.Server.Chat.Aggregate
{
    public partial record Channel
    {
        public static void UpdatePeerRoomInfo(ChannelUser user)
        {
            if (user.Channel.RoomType != PeerRoomType.Group)
            {
                return;
            }
            if (user.ClientRef.Info.IsRemoteClient)
            {
                return;
            }
            //#GPG!622
            var groupIdStr = user.Channel.Name.Split("!", StringSplitOptions.RemoveEmptyEntries)[1];
            if (!int.TryParse(groupIdStr, out var groupId))
            {
                throw new Chat.Exception("Peer room name is incorrect");
            }
            var roomInfo = QueryReport.V2.Application.StorageOperation.Persistance.GetPeerRoomsInfo(user.Channel.GameName, groupId);
            if (roomInfo.Count != 1)
            {
                throw new Chat.Exception($"No peer room info found with game name: {user.Channel.GameName}, group id:{groupId}");
            }
            roomInfo.First().NumberOfPlayers = user.Channel.Users.Count;
            // if(!IsUserExisted(user))
            // {
            //     var chan = ChannelManager.GetChannel(user.Info.PreviousJoinedChannel);
            //     if(chan is not null )
            //     {
            //         if(chan.RoomType == PeerRoomType.Group)
            //         {
            //             roomInfo.NumberOfGames
            //         }
            //     }
            // }
            QueryReport.V2.Application.StorageOperation.Persistance.UpdatePeerRoomInfo(roomInfo.First());
        }
        public static void UpdateChannelCache(ChannelUser user)
        {
            if (!user.ClientRef.Info.IsRemoteClient)
            {
                if (!Application.StorageOperation.Persistance.UpdateChannel(user.Channel))
                {
                    throw new Error.IRC.Channel.NoSuchChannelException("Update channel on redis fail.", user.Channel.Name);
                }
            }
        }
    }
}