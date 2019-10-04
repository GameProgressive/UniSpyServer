using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using GameSpyLib.Network;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace GameSpyLib.Network.TCP
{
    public abstract class TCPClientBase : IDisposable
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

        /// <summary>
        /// Indicates whether this object is disposed
        /// </summary>
        public bool Disposed { get; protected set; } = false;

        protected TCPClientBase(TCPStream stream, long connectionid)
        {
            // Set the connection ID
            ConnectionID = connectionid;

            SessionKey = 0;

            // Create our Client Stream
            Stream = stream;

            RemoteEndPoint = (IPEndPoint)stream.RemoteEndPoint;
        }

        ~TCPClientBase()
        {
            Dispose(false);
        }

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
        /// <summary>
        /// Send function for each server
        /// </summary>
        /// <param name="sendingBuffer"></param>
        public virtual void Send(string sendingBuffer, params object[] items)
        {
            Stream.SendAsync(sendingBuffer, items);
        }
        public virtual void ToLog(LogLevel level, string message)
        {
            message = Stream.Server.ServerName + message;
            LogWriter.Log.Write(level, message);
        }

        public abstract void SendServerChallenge(uint serverID);


        protected abstract void ClientDisconnected();

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed) return;
            //dispose managed resources
            if (disposing)
            {
                //wirte dispose method for child class
                if (!Stream.SocketClosed)
                    Stream.Dispose();
            }
            //dispose unmanaged resources

            Disposed = true;
        }


        protected virtual string RequstFormatConversion(string message)
        {
            message = message.Replace('-', '\\');
            int pos = message.IndexesOf("\\")[1];
            string temp = message.Substring(pos, 2);

            if (message.Substring(pos, 2) != "\\\\")
            {
                message = message.Insert(pos, "\\");
            }
            return message;
        }
    }

}
