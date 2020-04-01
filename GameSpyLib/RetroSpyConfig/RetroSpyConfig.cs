using Serilog.Events;
using System.Collections.Generic;

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
