using System.Linq;
using UniSpy.Server.Core.Logging;

namespace UniSpy.Server.Chat.Aggregate
{
    public partial record Channel
    {
        public static void UpdatePeerRoomInfo(ChannelUser user)
        {
            if (user.ClientRef.Info.IsRemoteClient)
            {
                return;
            }
            if (user.Channel.RoomType != PeerRoomType.Group)
            {
                return;
            }
            if (user.Info.GameName is null)
            {
                return;
            }
            if (!QueryReport.Application.Server.PeerGroupList.ContainsKey(user.Info.GameName))
            {
                return;
            }

            //#GPG!622
            // we just update this peer group
            var peerRoomInfo = QueryReport.V2.Application.StorageOperation.Persistance.GetPeerRoomsInfo(user.Channel.GameName, user.Channel.GroupId).FirstOrDefault();
            if (peerRoomInfo is null)
            {
                user.ClientRef.LogInfo("The peer room is not exist, we create it.");
                var grouplist = QueryReport.Application.Server.PeerGroupList[user.Channel.GameName];
                // check if there are missing peer rooms in redis
                var room = grouplist.Where(g => g.Groupid == user.Channel.GroupId).First();
                peerRoomInfo = new QueryReport.V2.Aggregate.Redis.PeerGroup.PeerRoomInfo(room.Game.Gamename, room.Groupid, room.Roomname);
            }
            peerRoomInfo.NumberOfPlayers = user.Channel.Users.Count;
            QueryReport.V2.Application.StorageOperation.Persistance.UpdatePeerRoomInfo(peerRoomInfo);
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