using System;
using System.Linq;
using System.Net;
using UniSpyServer.Servers.ServerBrowser.Abstraction;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.ServerBrowser.Entity.Structure
{
    public class ClientInfo : ClientInfoBase
    {
        public const string LocalIP0 = "localip0";
        public const string LocalIP1 = "localip1";
        public const string PrivatePort = "privateport";
        public const string HostPort = "hostport";
        public const string HostPortNumber = "6500";
        /// <summary>
        /// we are not gamespy,
        /// for simplicity we use hard coded challenge
        /// to reduce computation cost of our program
        /// </summary>
        public const string ServerChallenge = "0000000000";
        public const ushort QueryReportDefaultPort = 6500;
        public static byte[] HtonQueryReportDefaultPort => BitConverter.GetBytes(QueryReportDefaultPort).Reverse().ToArray();
        public string GameSecretKey { get; set; }
        public string ClientChallenge { get; set; }
        public AdHocRequest AdHocMessage { get; set; }
        public ClientInfo( )
        {
        }
    }
}