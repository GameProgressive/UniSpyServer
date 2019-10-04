using GameSpyLib.Common;
using GameSpyLib.Network;
using GameSpyLib.Network.TCP;
using PresenceSearchPlayer.Enumerator;
using System;
using System.Collections.Generic;

namespace PresenceSearchPlayer
{
    public class GPSPClient :TCPClientBase
    {    
        /// <summary>
        /// Event fired when the connection is closed
        /// </summary>
        public static event GPSPConnectionClosed OnDisconnected;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        public GPSPClient(TCPStream stream, long connectionid):base(stream,connectionid)
        {
            // Generate a unique name for this connection
            ConnectionID = connectionid;

            // Init a new client stream class
            Stream = stream;

            //determine whether gamespy request is finished
            Stream.IsMessageFinished += IsMessageFinished;

            // Read client message, and parse it into key value pairs
            Stream.OnDataReceived += ProcessData;

            //Dispose when client disconnected
            Stream.OnDisconnected += ClientDisconnected;

        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~GPSPClient()
        {
            if (!Disposed)
                Dispose();
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
                    //determine whether gamespy request is finished
                    Stream.IsMessageFinished -= IsMessageFinished;
                    // Read client message, and parse it into key value pairs
                    Stream.OnDataReceived -= ProcessData;
                    // If connection is still alive, disconnect user
                    Stream.OnDisconnected -= ClientDisconnected;
                    if (!Stream.SocketClosed)
                        Stream.Dispose();
                }               
            }
            catch { }


            // Call disconnect event
            OnDisconnected?.Invoke(this);

            Disposed = true;
        }

        /// <summary>
        /// This function is fired when data is received from a stream
        /// </summary>
        /// <param name="stream">The stream that sended the data</param>
        /// <param name="message">The message the stream sended</param>
        protected override void ProcessData(string message)
        {
            //message=@"\check\nick\FridiNaTor\email\fridij2007@icloud.com\pass\Clockwor1\gamename\fsw10hpc\final\";
            message = RequstFormatConversion(message);
            if (message[0] != '\\')
            {
                GameSpyUtils.SendGPError(this, GPErrorCode.Parse, "An invalid request was sended.");
                return;
            }

            string[] commands = message.Split("\\final\\");

            foreach (string command in commands)
            {
                if (command.Length < 1)
                    continue;

                // Read client message, and parse it into key value pairs
                string[] recieved = command.TrimStart('\\').Split('\\');
                Dictionary<string, string> dict = GameSpyUtils.ConvertGPResponseToKeyValue(recieved);

                CommandSwitcher.Switch(this, dict);
            }
        }


        public override void SendServerChallenge(uint serverID)
        {
            //
        }

        protected override void ClientDisconnected()
        {
            Dispose();
        }        
    }
}
