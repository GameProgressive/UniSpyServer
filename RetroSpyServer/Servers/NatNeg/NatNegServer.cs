using System;
using System.Net;
using GameSpyLib.Network;
using GameSpyLib.Database;
using GameSpyLib.Logging;
namespace RetroSpyServer.Servers.NatNeg
{
    public class NatNegServer : UdpServer
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbdriver">If choose sqlite for database you should pass the connection to server
        /// ,maybe NatNeg server dose not need connected to database.</param>
        /// <param name="bindTo"></param>
        /// <param name="MaxConnections"></param>
        NatNegServer(DatabaseDriver dbdriver, IPEndPoint bindTo, int MaxConnections) : base(bindTo, MaxConnections)
        {

        }
        protected override void OnException(Exception e) => LogWriter.Log.WriteException(e);


        protected override void ProcessAccept(UdpPacket packet)
        {
            throw new NotImplementedException();
            //TODO the natneg code here
        }
    }
}
