using GameSpyLib.Database.Entity;
using System.Xml.Serialization;

namespace GameSpyLib.XMLConfig
{
    public class DatabaseConfiguration
    {
        [XmlAttribute]
        public DatabaseEngine Type;

        public int Port;

        public string Hostname;

        public string Username;

        public string Password;

        public string Databasename;

        // SSL
        public string SslMode;

        public string SslCert;

        public string SslKey;

        public string SslCa;



    }
}
