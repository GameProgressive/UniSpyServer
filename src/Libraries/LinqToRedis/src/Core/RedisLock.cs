using System;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace UniSpy.LinqToRedis
{
    public class RedisLock : IDisposable
    {
        public TimeSpan LockTime { get; private set; }
        public TimeSpan? RetryTime { get; private set; }
        public TimeSpan? RetryInterval { get; private set; }
        public IDatabase Db { get; private set; }
        /// <summary>
        /// The token used to lock the object
        /// </summary>
        /// <value></value>
        public RedisValue LockToken { get; private set; } = Environment.MachineName;
        /// <summary>
        /// The key using to lock the object, original key is ChannelName:myfriend the lock key will be Lock_ChannelName:myfriend
        /// </summary>
        /// <value></value>
        public string LockKey { get; private set; }
        public RedisLock(TimeSpan lockTime, IDatabase db, RedisKeyValueObject key) : this(lockTime, null, null, db, key) { }
        public RedisLock(TimeSpan lockTime, TimeSpan? retryTime, TimeSpan? retryInterval, IDatabase db, RedisKeyValueObject key)
        {
            LockTime = lockTime;
            RetryTime = retryTime;
            RetryInterval = retryInterval;
            Db = db;
            LockKey = "Lock_" + key.FullKey;
        }

        public bool LockTake()
        {
            return Db.LockTake(LockKey, LockToken, LockTime);
        }
        public async Task<bool> LockTakeAsync()
        {
            return await Db.LockTakeAsync(LockKey, LockToken, LockTime);
        }
        public bool LockRelease()
        {
            return Db.LockRelease(LockKey, LockToken);
        }

        public void Dispose()
        {
            Db.LockRelease(LockKey, LockToken);
        }
    }
}