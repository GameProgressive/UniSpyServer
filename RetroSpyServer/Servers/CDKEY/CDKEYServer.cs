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
            // Start accepting remote connections
            base.StartAcceptAsync();
        }

        /// <summary>
        /// This function is fired when an exception occour in the server
        /// </summary>
        /// <param name="e">The exception to be thrown</param>
        protected override void OnException(Exception e) => LogWriter.Log.WriteException(e);

        /// <summary>
        /// Called when a connection comes in on the CDKey server
        /// </summary>
        protected override void ProcessAccept(UdpPacket handler)
        {
            KeyCheckResponse(handler);
        }


        /// <summary>
        /// Closes the underlying socket
        /// </summary>
        public void Shutdown()
        {
            base.ShutdownSocket();
            base.Dispose();
        }

        /// <summary>
        /// Check the cdkey validation and response
        /// </summary>
        /// <param name="handler"></param>
        /// known messages
        ///  \ka\ = keep alive from the game server every 20s, we don't care about this
        ///  \auth\ ... = authenticate cd key, this is what we care about
        ///  \disc\ ... = disconnect cd key, because there's checks if the cd key is in use, which we don't care about really, but we could if we wanted to
        /// </remarks>
        public void KeyCheckResponse(UdpPacket packet)
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
                        CheckCDKeyValidation(cdkeyrequest);
                        break;
                    case "disc":
                        DisconnectRequest(cdkeyrequest);
                        break;
                    default:
                        InvalidCDKeyRequest(cdkeyrequest);
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


        void CheckCDKeyValidation(string[] recieved)
        {
            Dictionary<string, string> Recv = GameSpyLib.Common.GamespyUtils.ConvertGPResponseToKeyValue(recieved);
            //check cdkey md5 in database

        }


        void DisconnectRequest(string[] recieved)
        {
            Dictionary<string, string> Recv = GameSpyLib.Common.GamespyUtils.ConvertGPResponseToKeyValue(recieved);
            // Handle, User disconnected from server
        }


        void InvalidCDKeyRequest(string[] recieved)
        {
            //write down the data we recieved through UDP packet
            LogWriter.Log.Write("Incomplete or Invalid CDKey Packet Received: {0}", LogLevel.Debug, recieved);

        }
    }
}
