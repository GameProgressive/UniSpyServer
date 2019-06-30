using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using GameSpyLib.Network;
using GameSpyLib.Database;
using GameSpyLib.Logging;
using GameSpyLib.Extensions;

namespace RetroSpyServer.Servers.CDKEY
{
    public class CDKeyServer : UdpServer
    {
        #region Server Setup Phase
        private CDKeyHelper helper;// This class implements the functions of gamespy cdkey check       
        bool replied = false;
        /// <summary>
        /// CDKeyServer
        /// </summary>
        /// <param name="dbdriver">if this server do not need databasedriver then do not handle this</param>
        /// <param name="bindTo">pass bind IP and Port in to CDKeyServer</param>
        /// <param name="MaxConnections">The Max client connections CDKeyServer can handle</param>
        public CDKeyServer(DatabaseDriver dbdriver, IPEndPoint bindTo, int MaxConnections) : base(bindTo, MaxConnections)
        {
            helper = new CDKeyHelper(dbdriver);
            // Start accepting remote connections
            base.StartAcceptAsync();
        }
        #endregion

        #region Server Functions
        /// <summary>
        /// Closes the underlying socket
        /// </summary>
        public void Shutdown()
        {
            base.ShutdownSocket();
            base.Dispose();
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
                string[] cdkeyrequest = decrypted.TrimStart('\\').Split('\\');

                switch (cdkeyrequest[0])
                {
                    //keep client alive request, we skip this
                    case "ka":
                    case "auth":
                    case "resp":
                    case "skey":
                        CheckCDKeyValidation(packet, cdkeyrequest);
                        break;
                    case "disc":
                        DisconnectRequest(packet, cdkeyrequest);
                        break;
                    default:
                        InvalidCDKeyRequest(packet, cdkeyrequest);
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
                    base.Release(packet.AsyncEventArgs);
            }

        }
        #endregion

        #region GameSpy CDKey Functions
        void CheckCDKeyValidation(UdpPacket packet, string[] recieved)
        {
            helper.IsCDKeyValid(this, packet, recieved);

        }

        void DisconnectRequest(UdpPacket packet, string[] recieved)
        {            
            // Handle, User disconnected from server
        }


        void InvalidCDKeyRequest(UdpPacket packet, string[] recieved)
        {
            LogWriter.Log.Write("Incomplete or Invalid CDKey Packet Received: {0}", LogLevel.Debug, recieved);
            if (!replied)
                base.Release(packet.AsyncEventArgs);
            //write down the data we recieved through UDP packet         

        }
        #endregion

    }
}
