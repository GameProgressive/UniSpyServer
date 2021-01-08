using UniSpyLib.Database;

namespace UniSpyLib.UniSpyConfig
{
    public class DatabaseConfig
    {
        public DatabaseType Type;
        public string RemoteAddress;
        public int RemotePort;
        public string UserName;
        public string Password;
        public string DatabaseName;
        public string SslMode;
        public string SslCert;
        public string SslKey;
        public string SslCa;
    }
}
