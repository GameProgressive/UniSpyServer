using GameSpyLib.Database.Entity;

namespace GameSpyLib.RetroSpyConfig
{
    public class DatabaseConfig
    {
        public DatabaseEngine Type;
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
