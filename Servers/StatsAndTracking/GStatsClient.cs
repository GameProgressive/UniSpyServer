using GameSpyLib.Common;
using GameSpyLib.Extensions;
using GameSpyLib.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StatsAndTracking
{
    public class GStatsClient : IDisposable
    {
        /// <summary>
        /// A unqie identifier for this connection
        /// </summary>
        public long ConnectionID;

        /// <summary>
        /// The clients socket network stream
        /// </summary>
        private TCPStream Stream;

        /// <summary>
        /// Indicates whether this object is disposed
        /// </summary>
        public bool Disposed { get; protected set; } = false;

        public string ServerChallengeKey { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        public GStatsClient(TCPStream stream, long connectionid)
        {
            // Generate a unique name for this connection
            ConnectionID = connectionid;

            // Init a new client stream class
            Stream = stream;

            //determine whether gamespy request is finished
            Stream.IsMessageFinished += IsMessageFinished;

            // Read client message, and parse it into key value pairs
            Stream.OnDataReceived += ProcessData;

            Stream.OnDisconnected += Dispose;

        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~GStatsClient()
        {
            if (!Disposed)
                Dispose();
        }

        public void Dispose()
        {
            // Only dispose once
            if (Disposed) return;
            Dispose(false);
        }

        /// <summary>
        /// Dispose method to be called by the server
        /// </summary>
        public void Dispose(bool DisposeEventArgs = false)
        {
            // Only dispose once
            if (Disposed) return;

            try
            {
                Stream.OnDisconnected -= Dispose;
                //determine whether gamespy request is finished
                Stream.IsMessageFinished -= IsMessageFinished;
                // Read client message, and parse it into key value pairs
                Stream.OnDataReceived -= ProcessData;
                // If connection is still alive, disconnect user
                if (!Stream.SocketClosed)
                    Stream.Close(DisposeEventArgs);
            }
            catch { }

            Disposed = true;
        }
        private bool IsMessageFinished(string message)
        {
            if (message.EndsWith("\\final\\"))
                return true;
            else
                return false;

        }
        /// <summary>
        /// This function is fired when data is received from a stream
        /// </summary>
        /// <param name="stream">The stream that sended the data</param>
        /// <param name="message">The message the stream sended</param>
        protected void ProcessData(string message)
        {
           
            message = message.Replace(@"\final\", "");
            string decodedmsg = GameSpyLib.Extensions.Enctypex.XorEncoding(message, 1);
            if (decodedmsg[0] != '\\')
            {
                return;
            }
            string[] recieved = decodedmsg.TrimStart('\\').Split('\\');
            Dictionary<string, string> dict = GameSpyUtils.ConvertGPResponseToKeyValue(recieved);

            CommandSwitcher.Switch(this, dict);

        }

        public void SendServerChallenge()
        {
            //38byte
            ServerChallengeKey = GameSpyLib.Common.Random.GenerateRandomString(38, GameSpyLib.Common.Random.StringType.Alpha);
            //string sendingBuffer = string.Format(@"\challenge\{0}\final\", ServerChallengeKey);
            //sendingBuffer = xor(sendingBuffer);
            string sendingBuffer = string.Format(@"\challenge\{0}", ServerChallengeKey);
            sendingBuffer = GameSpyLib.Extensions.Enctypex.XorEncoding(sendingBuffer, 1);
            sendingBuffer+= @"\final\";
            Stream.SendAsync(sendingBuffer);
        }
        public void Response(string sendingBuffer)
        {
            sendingBuffer = Enctypex.XorEncoding(sendingBuffer, 1);
            Stream.SendAsync(sendingBuffer);
        }
    }
}
