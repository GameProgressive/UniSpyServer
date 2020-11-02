using Serilog.Events;
using System.Collections.Generic;

namespace UniSpyLib.UniSpyConfig
{
    public class UniSpyConfig
    {
        public DatabaseConfig Database;
        public RedisConfig Redis;
        public List<ServerConfig> Servers;
        public LogEventLevel MinimumLogLevel;
    }
}
