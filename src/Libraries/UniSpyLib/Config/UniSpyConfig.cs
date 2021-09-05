using Serilog.Events;
using System.Collections.Generic;

namespace UniSpyLib.UniSpyConfig
{
    public class UniSpyConfig
    {
        public UniSpyDatabaseConfig Database;
        public UniSpyRedisConfig Redis;
        public List<UniSpyServerConfig> Servers;
        public LogEventLevel MinimumLogLevel;
    }
}
