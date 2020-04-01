using System;
using System.Collections.Generic;
using GameSpyLib.Logging;
using Serilog.Events;

namespace GameSpyLib.RetroSpyConfig
{
    public class RetroSpyConfig
    {
        public DatabaseConfig Database;
        public RedisConfig Redis;
        public List<ServerConfig> Servers;
        public LogEventLevel LogLevel;
    }
}
