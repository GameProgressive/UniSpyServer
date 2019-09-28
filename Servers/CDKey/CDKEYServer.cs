using GameSpyLib.Common;
using GameSpyLib.Database;
using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using GameSpyLib.Network.UDP;
using System;
using System.Collections.Generic;
using System.Net;

namespace CDKey
{
    public class CDKeyServer : UDPServer
    {


        public bool Replied = false;
        /// <summary>
        /// CDKeyServer
        /// </summary>
        /// <param name="dbdriver">if this server do not need databasedriver then do not handle this</param>
        /// <param name="bindTo">pass bind IP and Port in to CDKeyServer</param>
        /// <param name="MaxConnections">The Max client connections CDKeyServer can handle</param>
        public CDKeyServer(string serverName, DatabaseDriver dbdriver, IPEndPoint bindTo, int MaxConnections) : base(serverName, bindTo, MaxConnections)
        {
            CDKeyHandler.DBQuery = new CDKeyDBQuery(dbdriver);
            // Start accepting remote connections
            StartAcceptAsync();
        }


        /// <summary>
        /// Closes the underlying socket
        /// </summary>
        public void Shutdown()
        {
            ShutdownSocket();
            Dispose();
        }

        /// <summary>
        /// This function is fired when an exception occour in the server
        /// </summary>
        /// <param name="e">The exception to be thrown</param>
        protected override void OnException(Exception e) => LogWriter.Log.WriteException(e);

        /// <summary>
        ///  Called when a connection comes in on the CDKey server
        ///  known messages
        ///  \ka\ = keep alive from the game server every 20s, we don't care about this
        ///  \auth\ ... = authenticate cd key, this is what we care about
        ///  \disc\ ... = disconnect cd key, because there's checks if the cd key is in use, which we don't care about really, but we could if we wanted to
        /// </summary>
        protected override void ProcessAccept(UDPPacket packet)
        {
            // If we dont reply, we must manually release the EventArgs back to the pool
            Replied = false;
            try
            {
                // Decrypt message
                IPEndPoint remote = (IPEndPoint)packet.AsyncEventArgs.RemoteEndPoint;
                string decrypted = Enctypex.XorEncoding(packet.BytesRecieved, 0).Trim('\\');
                string[] recieved = decrypted.TrimStart('\\').Split('\\');
                Dictionary<string, string> recv = GameSpyUtils.ConvertGPResponseToKeyValue(recieved);

                switch (recieved[0])
                {
                    //keep client alive request, we skip this
                    case "ka":
                    case "auth":
                    case "resp":
                    case "skey":
                        CDKeyHandler.IsCDKeyValid(this, packet, recv);
                        break;
                    case "disc":
                        CDKeyHandler.DisconnectRequest(packet, recv);
                        break;
                    default:
                        CDKeyHandler.InvalidCDKeyRequest(this, packet, recv);
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
                if (!Replied)
                    Release(packet.AsyncEventArgs);
            }

        }
    }
}
