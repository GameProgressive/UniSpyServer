using System.Collections.Generic;
using UniSpy.Server.Chat.Aggregate;
using UniSpy.Server.Chat.Aggregate.Redis;
using UniSpy.Server.Chat.Application;
using UniSpy.Server.Core.Database.DatabaseModel;

namespace UniSpy.Server.Chat.Abstraction.Interface
{
    public interface IStorageOperation
    {
        public Dictionary<string, List<Grouplist>> PeerGroupList { get; }
        public ChannelCache.RedisClient ChannelCacheClient { get; }
        public ClientInfoCache.RedisClient ClientCacheClient { get; }
        (int userId, int profileId, bool emailVerified, bool banned) NickAndEmailLogin(string nickName, string email, string passwordHash);
        (int userId, int profileId, bool emailVerified, bool banned) UniqueNickLogin(string uniqueNick, int namespaceId);
        bool IsChannelExist(ChannelCache key);
        Channel GetChannel(ChannelCache key);
        void UpdateChannel(Channel channel);
        void RemoveChannel(Channel channel);
        void UpdateClient(Client client);
        void RemoveClient(Client client);
        bool IsClientExist(ClientInfoCache key);
    }
}