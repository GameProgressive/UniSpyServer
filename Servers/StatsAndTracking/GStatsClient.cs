using GameSpyLib.Common;
using GameSpyLib.Extensions;
using GameSpyLib.Network;
using GameSpyLib.Network.TCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StatsAndTracking
{
    public class GStatsClient :TCPClientBase, IDisposable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        public GStatsClient(TCPStream stream, long connectionid):base(stream,connectionid)
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
        /// Dispose method to be called by the server
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            // Only dispose once
            if (Disposed) return;

            try
            {
                if (disposing)
                {
                    Stream.OnDisconnected -= Dispose;
                    //determine whether gamespy request is finished
                    Stream.IsMessageFinished -= IsMessageFinished;
                    // Read client message, and parse it into key value pairs
                    Stream.OnDataReceived -= ProcessData;
                    // If connection is still alive, disconnect user
                    if (!Stream.SocketClosed)
                        Stream.Dispose();
                }
            }
            catch { }

            Disposed = true;
        }

        /// <summary>
        /// This function is fired when data is received from a stream
        /// </summary>
        /// <param name="stream">The stream that sended the data</param>
        /// <param name="message">The message the stream sended</param>
        protected override void ProcessData(string message)
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

        public override void SendServerChallenge(uint serverID)
        {
            //38byte
            ServerChallengeKey = GameSpyLib.Common.Random.GenerateRandomString(38, GameSpyLib.Common.Random.StringType.Alpha);
            //string sendingBuffer = string.Format(@"\challenge\{0}\final\", ServerChallengeKey);
            //sendingBuffer = xor(sendingBuffer);
            string sendingBuffer = string.Format(@"\challenge\{0}", ServerChallengeKey);
            sendingBuffer = GameSpyLib.Extensions.Enctypex.XorEncoding(sendingBuffer, 1);
            sendingBuffer += @"\final\";
            Stream.SendAsync(sendingBuffer);
        }
        public void Send(string sendingBuffer)
        {
            sendingBuffer = Enctypex.XorEncoding(sendingBuffer, 1);
            Stream.SendAsync(sendingBuffer);
        }

        protected override void ClientDisconnected()
        {
            throw new NotImplementedException();
        }
    }
}
