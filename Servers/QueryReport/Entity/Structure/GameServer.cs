using QueryReport.Entity.Structure.ReportData;
using System;
using System.Net;

namespace QueryReport.Entity.Structure
{
    /// <summary>
    /// This is the server 
    /// </summary>

    public class GameServer
    {
        /// <summary>
        /// Last valid heart beat packet time
        /// </summary>
        public DateTime LastRefreshed { get; set; }
        /// <summary>
        /// Last keep alive packet time
        /// </summary>
        public DateTime LastKeepAlive { get; set; }
        /// <summary>
        /// Last ping packet time
        /// </summary>
        public DateTime LastPing { get; set; }

        public EndPoint RemoteEndPoint { get; protected set; }

        public byte[] InstantKey = new byte[4];
        public GameServer(EndPoint endPoint, byte[] instantKey)
        {
            RemoteEndPoint = endPoint;
            instantKey.CopyTo(InstantKey, 0);
        }


        public bool IsValidated = false;
        public byte[] RemotePort
        {
            get { return BitConverter.GetBytes(((IPEndPoint)RemoteEndPoint).Port); }
        }
        public byte[] RemoteIP
        {
            get { return ((IPEndPoint)RemoteEndPoint).Address.GetAddressBytes(); }
        }


        public ServerKey ServerKeyValue = new ServerKey();
        public PlayerKey  PlayerKeyValue = new PlayerKey();
        public TeamKey TeamKeyValue = new TeamKey();

    }
}
