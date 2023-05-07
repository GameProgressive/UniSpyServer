using System.Linq;
using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Core.Database.DatabaseModel;
using UniSpy.Server.QueryReport.Aggregate.Redis.Channel;

namespace UniSpy.Server.Chat.Application
{
    public sealed class StorageOperation : IStorageOperation
    {
        public static IStorageOperation Persistance = new StorageOperation();
        private static RedisClient _redisClient = new RedisClient();
        public (int userId, int profileId, bool emailVerified, bool banned) NickAndEmailLogin(string nickName, string email, string passwordHash)
        {
            using (var db = new UniSpyContext())
            {
                var result = from u in db.Users
                             join p in db.Profiles on u.UserId equals p.Userid
                             where u.Email == email
                             && p.Nick == nickName
                             && u.Password == passwordHash
                             select new
                             {
                                 userId = u.UserId,
                                 profileId = p.ProfileId,
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
                             join p in db.Profiles on n.ProfileId equals p.ProfileId
                             join u in db.Users on p.Userid equals u.UserId
                             where n.Uniquenick == uniqueNick
                             && n.NamespaceId == namespaceId
                             select new
                             {
                                 userId = u.UserId,
                                 profileId = p.ProfileId,
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
    }
}