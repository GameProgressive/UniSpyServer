using System;
using System.Collections.Generic;
using System.Linq;
using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Chat.Aggregate;
using UniSpy.Server.Chat.Aggregate.Redis;
using UniSpy.Server.Core.Database.DatabaseModel;

namespace UniSpy.Server.Chat.Application
{
    public sealed class StorageOperation : IStorageOperation
    {
        public static IStorageOperation Persistance = new StorageOperation();
        /// <summary>
        /// The peer group list in memory
        /// </summary>
        Dictionary<string, List<Grouplist>> IStorageOperation.PeerGroupList => _peerGroupList;
        private readonly Dictionary<string, List<Grouplist>> _peerGroupList = GetAllGroupList();
        public ChannelCache.RedisClient ChannelCacheClient { get; } = new();
        public ClientInfoCache.RedisClient ClientCacheClient { get; } = new();


        public StorageOperation()
        {
        }

        public (int userId, int profileId, bool emailVerified, bool banned) NickAndEmailLogin(string nickName, string email, string passwordHash)
        {
            using (var db = new UniSpyContext())
            {
                var result = from u in db.Users
                             join p in db.Profiles on u.Userid equals p.Userid
                             where u.Email == email
                             && p.Nick == nickName
                             && u.Password == passwordHash
                             select new
                             {
                                 userId = u.Userid,
                                 profileId = p.Profileid,
                                 emailVerified = u.Emailverified,
                                 banned = u.Banned
                             };

                if (result.Count() != 1)
                {
                    throw new Chat.Exception($"Can not find user with nickname:{nickName} in database.");
                }
                return (userId: result.First().userId,
                        profileId: result.First().profileId,
                        emailVerified: (bool)result.First().emailVerified,
                        banned: result.First().banned);
            }
        }
        public (int userId, int profileId, bool emailVerified, bool banned) UniqueNickLogin(string uniqueNick, int namespaceId)
        {
            using (var db = new UniSpyContext())
            {
                var result = from n in db.Subprofiles
                             join p in db.Profiles on n.Profileid equals p.Profileid
                             join u in db.Users on p.Userid equals u.Userid
                             where n.Uniquenick == uniqueNick
                             && n.Namespaceid == namespaceId
                             select new
                             {
                                 userId = u.Userid,
                                 profileId = p.Profileid,
                                 uniquenick = n.Uniquenick,
                                 emailVerified = u.Emailverified,
                                 banned = u.Banned
                             };
                if (result.Count() != 1)
                {
                    throw new Chat.Exception($"Can not find user with uniquenick:{uniqueNick} in database.");
                }
                // _result.ProfileId = result.First().profileid;
                // _result.UserID = result.First().userid;
                // Select(
                return (userId: result.First().userId,
                        profileId: result.First().profileId,
                        emailVerified: (bool)result.First().emailVerified,
                        banned: result.First().banned);
            }
        }

        public bool IsChannelExist(ChannelCache key)
        {
            return ChannelCacheClient.Context.Count(c => c.ChannelName == key.ChannelName && c.GameName == key.GameName) == 1 ? true : false;
        }

        public Channel GetChannel(ChannelCache key)
        {
            var result = ChannelCacheClient.Context.Where(c => c.ChannelName == key.ChannelName && c.GameName == key.GameName).FirstOrDefault();
            return result?.Channel;
        }

        public void UpdateChannel(Channel channel)
        {
            var data = new ChannelCache
            {
                ChannelName = channel.Name,
                GameName = channel.GameName,
                Channel = channel
            };
            ChannelCacheClient.SetValue(data);
        }

        public void RemoveChannel(Channel channel)
        {
            var data = new ChannelCache
            {
                ChannelName = channel.Name,
            };
            ChannelCacheClient.DeleteKeyValue(data);
        }

        public void UpdateClient(IShareClient client)
        {
            // we do not update client info when its nickname is not registered
            if (client.Info.NickName is null)
            {
                return;
            }
            var data = new ClientInfoCache
            {
                NickName = client.Info.NickName,
                Info = client.Info
            };
            ClientCacheClient.SetValue(data);
        }

        public void RemoveClient(IShareClient client)
        {
            var data = new ClientInfoCache
            {
                NickName = client.Info.NickName
            };
            ClientCacheClient.DeleteKeyValue(data);
        }

        public bool IsClientExist(ClientInfoCache key)
        {
            return ClientCacheClient.Context.Count(c => c.NickName == key.NickName) == 1 ? true : false;
        }

        private static Dictionary<string, List<Grouplist>> GetAllGroupList()
        {
            using (var db = new UniSpyContext())
            {
                var result = from g in db.Games
                             join gl in db.Grouplists on g.Gameid equals gl.Gameid
                             select new Grouplist
                             {
                                 Game = g,
                                 Gameid = g.Gameid,
                                 Groupid = gl.Groupid,
                                 Roomname = gl.Roomname
                             };
                var result2 = from g in result
                              group g by g.Game.Gamename into dd
                              select new KeyValuePair<string, List<Grouplist>>(dd.Key, dd.ToList());

                var data = result2.ToDictionary(x => x.Key, x => x.Value);
                return data;
            }
        }
    }
}