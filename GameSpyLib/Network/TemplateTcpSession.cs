using GameSpyLib.Common;
using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using NetCoreServer;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace GameSpyLib.Network
{
    /// <summary>
    /// This is a template class that helps creating a TCP Session (formerly TCP stream) with logging functionality and ServerName, as required in the old network stack.
    /// </summary>
    public class TemplateTcpSession : TcpSession
    {
        public string ServerName;
        /// <summary>
        /// Some server requires that clients should disconnect after send, we use this to determine whether disconnecting clients
        /// </summary>
        protected bool DisconnectAfterSend = false;
        public EndPoint Remote;
        public TemplateTcpSession(TemplateTcpServer server) : base(server)
        {
            ServerName = server.ServerName;
        }

        /// <summary>
        /// Handle error notification
        /// </summary>
        /// <param name="error">Socket error code</param>
        protected override void OnError(SocketError error)
        {
            LogWriter.Log.Write(LogLevel.Error, "{0} Error: {1}", ServerName, Enum.GetName(typeof(SocketError), error));
        }

        /// <summary>
        /// Send data to the client (asynchronous)
        /// </summary>
        /// <param name="buffer">Buffer to send</param>
        /// <param name="offset">Buffer offset</param>
        /// <param name="size">Buffer size</param>
        /// <returns>'true' if the data was successfully sent, 'false' if the session is not connected</returns>
        /// <remarks>
        /// We override this method in order to let it print the data it transmits, please call "base.SendAsync" in your overrided function.
        /// </remarks>
        public override bool SendAsync(byte[] buffer, long offset, long size)
        {
            if (LogWriter.Log.DebugSockets)
                LogWriter.Log.Write(LogLevel.Debug, "{0}[Send] TCP data: {1}", ServerName, Encoding.UTF8.GetString(buffer));

            bool returnValue = base.SendAsync(buffer, offset, size);

            if (DisconnectAfterSend)
                Disconnect();

            return returnValue;
        }

        /// <summary>
        /// Send data to the client (synchronous)
        /// </summary>
        /// <param name="buffer">Buffer to send</param>
        /// <param name="offset">Buffer offset</param>
        /// <param name="size">Buffer size</param>
        /// <returns>Size of sent data</returns>
        /// <remarks>
        /// We override this method in order to let it print the data it transmits, please call "base.Send" in your overrided function.
        /// </remarks>
        public override long Send(byte[] buffer, long offset, long size)
        {
            if (LogWriter.Log.DebugSockets)
                LogWriter.Log.Write(LogLevel.Debug, "{0}[Send] TCP data: {1}", ServerName, Encoding.UTF8.GetString(buffer));

            long returnValue = base.Send(buffer, offset, size);

            if (DisconnectAfterSend)
                Disconnect();

            return returnValue;
        }
        /// <summary>
        /// Our method to receive message and print in the console
        /// </summary>
        /// <param name="recv">message we recieved</param>
        protected virtual void OnReceived(string message)
        { }

        /// <summary>
        /// Handle buffer received notification
        /// </summary>
        /// <param name="buffer">Received buffer</param>
        /// <param name="offset">Received buffer offset</param>
        /// <param name="size">Received buffer size</param>
        /// <remarks>
        /// Notification is called when another chunk of buffer was received from the client
        /// We override this method in order to let it print the data it transmits, please call "base.OnReceived" in your overrided function
        /// </remarks>
        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            if (size > 2048)
            {
                ToLog("[Spam] client spam we ignored!");
                return;
            }
            if (LogWriter.Log.DebugSockets)
                LogWriter.Log.Write(LogLevel.Debug, "{0}[Recv] TCP data: {1}", ServerName, Encoding.UTF8.GetString(buffer, 0, (int)size));

            OnReceived(Encoding.UTF8.GetString(buffer, 0, (int)size));
        }

        protected override void OnConnected()
        {
            Remote = Socket.RemoteEndPoint;
            ToLog($"[Conn] ID:{Id} IP:{Remote.ToString()}");
            base.OnConnected();
        }
        protected override void OnDisconnected()
        {
            ToLog($"[Disc] ID:{Id} IP:{Remote.ToString()}");
            base.OnDisconnected();
        }

        public virtual void ToLog(string text)
        {
            ToLog(LogLevel.Info, text);
        }
        public virtual void ToLog(LogLevel level, string text)
        {
            text = ServerName + text;
            LogWriter.Log.Write(text, level);
        }

        public virtual void UnknownDataRecived(string text, Dictionary<string, string> recv)
        {
            string errorMsg = string.Format("Received unknown data: {0}", text);
            GameSpyUtils.PrintReceivedGPDictToLogger(recv);
            ToLog(errorMsg);
        }

        public virtual void UnknownDataRecived(Dictionary<string, string> recv)
        {
            GameSpyUtils.PrintReceivedGPDictToLogger(recv);
        }

        public virtual string RequstFormatConversion(string message)
        {
            if (message.Contains("login"))
            {
                message = message.Replace(@"\-", @"\");
                message = message.Replace('-', '\\');

                int pos = message.IndexesOf("\\")[1];

                if (message.Substring(pos, 2) != "\\\\")
                {
                    message = message.Insert(pos, "\\");
                }
                return message;
            }
            else
            {
                return message;
            }
            
        }
    }
}
