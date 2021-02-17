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
        /// <param name="endPoint"></param>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        /// <param name="nickName"></param>
        /// <param name="profileID"></param>
        /// <param name="uniqueNick"></param>
        /// <param name="namespaceID"></param>
        /// <returns></returns>
        public static string BuildFullKey(Guid guid, uint userID, string userName, string nickName, uint profileID,uint subProfileID, string uniqueNick, uint namespaceID)
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
    }
}
