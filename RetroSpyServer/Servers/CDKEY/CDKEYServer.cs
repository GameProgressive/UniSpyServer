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
    public class CDKEYServer : GameSpyUDPConnector
    {

        CDKEYClient m_cdkeyClient;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbdriver">if this server do not need databasedriver then do not handle this</param>
        /// <param name="bindTo">pass bind IP and Port in to CDKEYServer</param>
        /// <param name="MaxConnections">The Max client connections CDKEYServer can handle</param>
        public CDKEYServer(DatabaseDriver dbdriver, IPEndPoint bindTo, int MaxConnections) : base(bindTo, MaxConnections)
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
        /// known messages
        ///  \ka\ = keep alive from the game server every 20s, we don't care about this
        ///  \auth\ ... = authenticate cd key, this is what we care about
        ///  \disc\ ... = disconnect cd key, because there's checks if the cd key is in use, which we don't care about really, but we could if we wanted to
        /// </remarks>
        protected override void ProcessAccept(GameSpyUDPHandler handler)
        {
            // If we dont reply, we must manually release the EventArgs back to the pool
            bool replied = false;
            try
            {
                // Decrypt message
                IPEndPoint remote = (IPEndPoint)handler.AsyncEventArgs.RemoteEndPoint;
                string decrypted = Enctypex.XOR(Encoding.UTF8.GetString(handler.BytesRecieved)).Trim('\\');

                // Ignore keep alive pings
                if (!decrypted.StartsWith("ka"))
                {
                    Dictionary<string, string> recv = m_cdkeyClient.ConvertToKeyValue(decrypted.Split('\\'));
                    if (recv.ContainsKey("auth") && recv.ContainsKey("resp") && recv.ContainsKey("skey"))
                    {
                        LogWriter.Log.Write("CDKey Check Requested from: {0}:{1}", LogLevel.Debug, remote.Address, remote.Port);

                        // Normally you would check the CD key database for the CD key MD5, but we arent Gamespy, we dont care
                        string cdkeyAvaliableReply = String.Format(@"\uok\\cd\{0}\skey\{1}", recv["resp"].Substring(0, 32), recv["skey"]);

                        // Set new packet contents, and send a reply
                        handler.SetBufferContents(Encoding.UTF8.GetBytes(Enctypex.XOR(cdkeyAvaliableReply)));
                        base.ReplyAsync(handler);
                        replied = true;
                    }
                    else if (recv.ContainsKey("disc"))
                    {
                        // Handle, User disconnected from server
                    }
                    else
                    {
                        LogWriter.Log.Write("Incomplete or Invalid CDKey Packet Received: {0}", LogLevel.Debug, decrypted);
                    }
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
                    base.Release(handler.AsyncEventArgs);
            }
        }


        /// <summary>
        /// Closes the underlying socket
        /// </summary>
        public void Shutdown()
        {
            base.ShutdownSocket();
            base.Dispose();
        }
    }
}
