using GameSpyLib.Common;
using GameSpyLib.Logging;
using GameSpyLib.Network;
using Chat.Application;
using System;
using System.Collections.Generic;
using GameSpyLib.Network.TCP;

namespace Chat
{
    public class ChatClient : TCPClientBase
    {
        /// <summary>
        /// Event fired when the connection is closed
        /// </summary>
        public static event ChatConnectionClosed OnDisconnect;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        public ChatClient(TCPStream stream, long connectionid) : base(stream, connectionid)
        {
            // Generate a unique name for this connection
            ConnectionID = connectionid;

            // Init a new client stream class
            Stream = stream;
            //determine whether gamespy request is finished
            Stream.IsMessageFinished += IsMessageFinished;

            // Read client message, and parse it into key value pairs
            Stream.OnDataReceived += ProcessData;

            //Dispose stream when a client is disconnected
            Stream.OnDisconnected += Dispose;

        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~ChatClient()
        {
                Dispose(false);
        }


        /// <summary>
        /// Dispose method to be called by the server
        /// </summary>
        public override void Dispose(bool disposing)
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
                    Stream.Close();
            }
            catch { }
            // Call disconnect event
            OnDisconnect?.Invoke(this);

            Disposed = true;
        }
        /// <summary>
        /// This function is fired when data is received from a stream
        /// </summary>
        /// <param name="stream">The stream that sended the data</param>
        /// <param name="message">The message the stream sended</param>
        protected override void ProcessData(string message)
        {
            string[] temp = message.Trim(' ').Split(' ');

            //LogWriter.Log.Write("[CHAT] Recv " + message, LogLevel.Error);
            //Stream.SendAsync("PING capricorn.goes.here :123456");
            ChatHandler.Crypt(this, temp);
        }

        protected override void ClientDisconnected()
        {
            throw new NotImplementedException();
        }
    }
}

