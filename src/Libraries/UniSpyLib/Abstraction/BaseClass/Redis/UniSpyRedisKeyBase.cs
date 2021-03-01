using System;

namespace UniSpyLib.Abstraction.BaseClass.Redis
{
    public class UniSpyRedisKeyBase
    {
        public Guid ServerID { get; set; }
        public UniSpyRedisKeyBase()
        {
        }

        public virtual string BuildFullKey()
        {
            string redisKey = "";
            foreach (var property in GetType().GetFields())
            {
                if (property.GetValue(this) == null)
                {
                    throw new ArgumentNullException();
                }
                redisKey += $"{property.Name}:{property.GetValue(this)} ";
            }
            return redisKey;
        }

        public virtual string BuildSearchKey()
        {
            string redisKey = "*";
            foreach (var property in GetType().GetFields())
            {
                if (property.GetValue(this) == null)
                {
                    continue;
                }
                redisKey += $"{property.Name}:{property.GetValue(this)}*";
            }
            return redisKey;
        }
    }
}
