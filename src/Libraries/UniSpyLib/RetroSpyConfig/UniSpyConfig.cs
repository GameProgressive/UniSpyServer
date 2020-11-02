using Serilog.Events;
using System.Collections.Generic;

namespace UniSpyLib.RetroSpyConfig
{
    public class UniSpyConfig
    {
        public DatabaseConfig Database;
        public RedisConfig Redis;
        public List<ServerConfig> Servers;
        public LogEventLevel MinimumLogLevel;
    }
}
