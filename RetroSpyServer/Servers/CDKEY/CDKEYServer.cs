using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using GameSpyLib.Network;
using GameSpyLib.Database;
using GameSpyLib.Logging;
using GameSpyLib.Extensions;
using GameSpyLib.Common;

namespace RetroSpyServer.Servers.CDKey
{
    public class CDKeyServer : UdpServer
    {
        #region Server Setup Phase

        public bool replied = false;
        /// <summary>
        /// CDKeyServer
        /// </summary>
        /// <param name="dbdriver">if this server do not need databasedriver then do not handle this</param>
        /// <param name="bindTo">pass bind IP and Port in to CDKeyServer</param>
        /// <param name="MaxConnections">The Max client connections CDKeyServer can handle</param>
        public CDKeyServer(string serverName, DatabaseDriver dbdriver, IPEndPoint bindTo, int MaxConnections) : base(serverName, bindTo, MaxConnections)
        {
            CDKeyHelper.DBQuery = new DBQueries.CDKeyDBQuery(dbdriver);
            // Start accepting remote connections
            StartAcceptAsync();
        }
        #endregion

        #region Server Functions
        /// <summary>
        /// Closes the underlying socket
        /// </summary>
        public void Shutdown()
        {
            ShutdownSocket();
            Dispose();
        }
        public void Release(UdpPacket packet)
        {
            Release(packet.AsyncEventArgs);
        }
        /// <summary>
        /// This function is fired when an exception occour in the server
        /// </summary>
        /// <param name="e">The exception to be thrown</param>
        protected override void OnException(Exception e) => LogWriter.Log.WriteException(e);
        #endregion

        #region Client Request Handle
        /// <summary>
        ///  Called when a connection comes in on the CDKey server
        ///  known messages
        ///  \ka\ = keep alive from the game server every 20s, we don't care about this
        ///  \auth\ ... = authenticate cd key, this is what we care about
        ///  \disc\ ... = disconnect cd key, because there's checks if the cd key is in use, which we don't care about really, but we could if we wanted to
        /// </summary>
        protected override void ProcessAccept(UdpPacket packet)
        {
            // If we dont reply, we must manually release the EventArgs back to the pool
            replied = false;
            try
            {
                // Decrypt message
                IPEndPoint remote = (IPEndPoint)packet.AsyncEventArgs.RemoteEndPoint;
                string decrypted = Enctypex.XOR(Encoding.UTF8.GetString(packet.BytesRecieved)).Trim('\\');
                string[] recieved = decrypted.TrimStart('\\').Split('\\');
                Dictionary<string, string> dict = GameSpyUtils.ConvertGPResponseToKeyValue(recieved);

                switch (recieved[0])
                {
                    //keep client alive request, we skip this
                    case "ka":
                    case "auth":
                    case "resp":
                    case "skey":
                        CDKeyHelper.IsCDKeyValid(this, packet, dict);
                        break;
                    case "disc":
                        CDKeyHelper.DisconnectRequest(packet, dict);
                        break;
                    default:
                        CDKeyHelper.InvalidCDKeyRequest(this, packet, dict);
                        break;
                }
            }
            catch (Exception e)
            {
                LogWriter.Log.WriteException(e);
            }
            finally
            {
                // Release so that we can pool the EventArgs to be used on another connection
                if (!replied)
                    Release(packet.AsyncEventArgs);
            }

        }
        #endregion




    }
}
