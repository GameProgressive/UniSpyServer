using GameSpyLib.Database;

namespace RetroSpyServer.Application
{
    /// <summary>
    /// This class represents the configuration of a server
    /// </summary>
    public class ServerConfiguration
    {
        /// <summary>
        /// The IP that the server will bind
        /// </summary>
        public string ip = "localhost";

        /// <summary>
        /// The port that the server will use
        /// </summary>
        public int port = 0;

        /// <summary>
        /// Max connections available for the server
        /// </summary>
        public int maxConnections = 0;

        /// <summary>
        /// If this variable is setted to true, then the server
        /// will not start
        /// </summary>
        public bool disable = false;
    };

    /// <summary>
    /// This class rapresents the configuration of the database
    /// </summary>
    public class DatabaseConfiguration
    {
        /// <summary>
        /// Contains the database name
        /// </summary>
        public static string name = ":memory:";

        /// <summary>
        /// Contains the IP of the database
        /// </summary>
        public static string host = "127.0.0.1";

        /// <summary>
        /// Contains the username that will be used to login in the database
        /// </summary>
        public static string username = "";

        /// <summary>
        /// Contains the password that will be used to login in the database
        /// </summary>
        public static string password = "";

        /// <summary>
        /// The type of the database that will be used
        /// </summary>
        public static DatabaseEngine type = DatabaseEngine.Sqlite;

        /// <summary>
        /// The port of the database the server will connect
        /// </summary>
        public static int port = 3306;
    };
}
