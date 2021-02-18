using System;
using System.Net;
using PresenceConnectionManager.Structure.Data;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Extensions;

namespace PresenceConnectionManager.Handler.SystemHandler.Redis
{
    internal class PCMUserInfoRedisOperator : UniSpyRedisOperatorBase<PCMUserInfo>
    {
        static PCMUserInfoRedisOperator()
        {
            _dbNumber = RedisDBNumber.GamePresence;
        }
        /// <summary>
        /// We define the parameters that PCM redis needed.
        /// </summary>
        /// <returns>The full key in redis</returns>
        public static string BuildFullKey(Guid guid, uint userID, string userName, string nickName, uint profileID, uint subProfileID, string uniqueNick, uint namespaceID)
        {
            return BuildFullKey(
                $"Guid:{guid}",
                $"UserID:{userID}",
                $"UserName:{userName}",
                $"NickName:{nickName}",
                $"ProfileID:{profileID}",
                $"SubProfileID:{subProfileID}",
                $"UniqueNick:{uniqueNick}",
                $"NameSpaceID:{namespaceID}");
        }
        public static string BuildSearchKey(uint subProfileID)
        {
            return BuildSearchKey($"SubProfileID:{subProfileID}");
        }
        public static string BuildSearchKey(string uniqueNick, uint nameSpaceID)
        {
            return BuildSearchKey(
                $"UniqueNick:{uniqueNick}",
                $"NameSpaceID:{nameSpaceID}");
        }
        public static string BuildSearchKey(uint userID, uint profileID, uint nameSpaceID)
        {
            return BuildSearchKey(
                $"UserID:{userID}",
                $"ProfileID:{profileID}",
                $"NameSpaceID:{nameSpaceID}");
        }
    }
}
