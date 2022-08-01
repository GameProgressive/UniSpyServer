// using Newtonsoft.Json;
// using System;
// using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Redis;

// namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.SystemHandler.Redis
// {
//     public sealed class UserInfoRedisKey : RedisKey
//     {
//         [JsonProperty(Order = -2)]
//         public Guid ServerID { get; set; }
//         public string SessionHashValue { get; set; }
//         public UserInfoRedisKey()
//         {
//             Db = UniSpyServer.UniSpyLib.Extensions.DbNumber.GamePresence;
//         }
//     }
// }