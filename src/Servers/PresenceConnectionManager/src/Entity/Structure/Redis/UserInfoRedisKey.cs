// using Newtonsoft.Json;
// using System;
// using UniSpy.Server.Core.Abstraction.BaseClass.Redis;

// namespace UniSpy.Server.PresenceConnectionManager.Handler.SystemHandler.Redis
// {
//     public sealed class UserInfoRedisKey : RedisKey
//     {
//         [JsonProperty(Order = -2)]
//         public Guid ServerID { get; set; }
//         public string SessionHashValue { get; set; }
//         public UserInfoRedisKey()
//         {
//             Db = UniSpy.Server.Core.Extensions.DbNumber.GamePresence;
//         }
//     }
// }