using GameSpyLib.Logging;
using GameSpyLib.Network;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace GameSpyLib.Network.TCP
{
    public abstract class TCPClientBase:IDisposable
    {
        /// <summary>
        /// A unqie identifier for this connection
        /// </summary>
        public long ConnectionID { get; protected set; }

        /// <summary>
        /// The clients socket network stream
        /// </summary>
        public TCPStream Stream { get; protected set; }

        /// <summary>
        /// The Servers challange key, sent when the client first connects.
        /// This is used as part of the hash used to "proove" to the client
        /// that the password in our database matches what the user enters
        /// </summary>
        public string ServerChallengeKey { get; protected set; }

        /// <summary>
        /// The TCPClient's Endpoint
        /// </summary>
        public IPEndPoint RemoteEndPoint { get; protected set; }

        protected TCPClientBase(TCPStream stream, long connectionid)
        {
            // Set the connection ID
            ConnectionID = connectionid;

            SessionKey = 0;

            // Create our Client Stream
            Stream = stream;

            RemoteEndPoint = (IPEndPoint)stream.RemoteEndPoint;
        }

        /// <summary>
        /// Indicates whether this object is disposed
        /// </summary>
        public bool Disposed { get; protected set; } = false;

        /// <summary>
        /// The users session key
        /// </summary>
        public ushort SessionKey { get; set; }
        protected virtual bool IsMessageFinished(string message)
        {
            if (message.EndsWith("\\final\\"))
                return true;
            else
                return false;
        }
        protected abstract void ProcessData(string message);
        public abstract void Send(string sendingBuffer);

        public abstract void SendServerChallenge(uint serverID);

        protected abstract void ClientDisconnected();

        public virtual void ToLog(LogLevel level, string status, string statusinfo, string message, params object[] items)
        {
            string temp1 = string.Format(message, items);
            string temp2 = string.Format("{0} [{1}] {2}: {3}", Stream.SocketManager.ServerName, status, statusinfo, temp1);
            LogWriter.Log.Write(LogLevel.Debug, temp2);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed) return;
            //dispose managed resources
            if (disposing)
            {
                //wirte dispose method for child class
                if (!Stream.SocketClosed)
                    Stream.Close(true);
            }

            Disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

}
