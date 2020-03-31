using System;
using System.Collections.Generic;
using GameSpyLib.Logging;

namespace GameSpyLib.JsonConfig
{
    public class RetroSpyConfig
    {
        public DatabaseConfig Database;
        public RedisConfig Redis;
        public List<ServerConfig> Servers;
        public LogLevel LogLevel;
    }
}
