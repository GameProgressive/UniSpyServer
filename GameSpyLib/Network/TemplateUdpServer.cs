using GameSpyLib.Logging;
using NetCoreServer;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace GameSpyLib.Network
{
    /// <summary>
    /// This is a template class that helps creating a UDP Server with logging functionality and ServerName, as required in the old network stack.
    /// </summary>
    public class TemplateUdpServer : UdpServer
    {
        /// <summary>
        /// The name of the server that is started, used primary in logging functions.
        /// </summary>
        public string ServerName { get; private set; }


        /// <summary>
        /// Initialize UDP server with a given IP address and port number
        /// </summary>
        /// <param name="serverName">The name of the server that will be started</param>
        /// <param name="address">IP address</param>
        /// <param name="port">Port number</param>
        public TemplateUdpServer(string serverName, IPEndPoint endpoint) : base(endpoint)
        {
            ServerName = '[' + serverName + ']' + ' ';
            Start();
        }

        protected override void OnStarted()
        {
            // Start receive datagrams
            ReceiveAsync();
        }

        /// <summary>
        /// Initialize UDP server with a given IP address and port number
        /// </summary>
        /// <param name="serverName">The name of the server that will be started</param>
        /// <param name="address">IP address</param>
        /// <param name="port">Port number</param>
        public TemplateUdpServer(string serverName, IPAddress address, int port) : base(address, port)
        {
            ServerName = '[' + serverName + ']' + ' ';
            Start();
        }

        /// <summary>
        /// Handle error notification
        /// </summary>
        /// <param name="error">Socket error code</param>
        protected override void OnError(SocketError error)
        {
            LogWriter.Log.Write(LogLevel.Error, "{0}Error: {1}", ServerName, Enum.GetName(typeof(SocketError), error));
        }

        /// <summary>
        /// Send datagram to the given endpoint (asynchronous)
        /// </summary>
        /// <param name="endpoint">Endpoint to send</param>
        /// <param name="buffer">Datagram buffer to send</param>
        /// <param name="offset">Datagram buffer offset</param>
        /// <param name="size">Datagram buffer size</param>
        /// <returns>'true' if the datagram was successfully sent, 'false' if the datagram was not sent</returns>
        /// <remarks>
        /// We override this method in order to let it print the data it transmits, please call "base.SendAsync" in your overrided function.
        /// </remarks>
        public override bool SendAsync(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            if (LogWriter.Log.DebugSockets)
                LogWriter.Log.Write(LogLevel.Debug, "{0}[Send] UDP data: {1}", ServerName, Encoding.UTF8.GetString(buffer));

            return base.SendAsync(endpoint, buffer, offset, size);
        }
        public bool BaseSendAsync(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            return base.SendAsync(endpoint, buffer, offset, size);
        }
        /// <summary>
        /// Send datagram to the given endpoint (synchronous)
        /// </summary>
        /// <param name="endpoint">Endpoint to send</param>
        /// <param name="buffer">Datagram buffer to send</param>
        /// <param name="offset">Datagram buffer offset</param>
        /// <param name="size">Datagram buffer size</param>
        /// <returns>Size of sent datagram</returns>
        /// <remarks>
        /// We override this method in order to let it print the data it transmits, please call "base.Send" in your overrided function.
        /// </remarks>
        public override long Send(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            if (LogWriter.Log.DebugSockets)
                LogWriter.Log.Write(LogLevel.Debug, "{0}[Send] UDP data: {1}", ServerName, Encoding.UTF8.GetString(buffer));

            return base.Send(endpoint, buffer, offset, size);
        }
        public long BaseSend(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            return base.Send(endpoint, buffer, offset, size);
        }
        protected override void OnSent(EndPoint endpoint, long sent)
        {
            // Continue receive datagrams
            ReceiveAsync();
        }

        protected virtual void OnReceived(EndPoint endPoint, byte[] message)
        {

        }
        /// <summary>
        /// Handle datagram received notification
        /// </summary>
        /// <param name="endpoint">Received endpoint</param>
        /// <param name="buffer">Received datagram buffer</param>
        /// <param name="offset">Received datagram buffer offset</param>
        /// <param name="size">Received datagram buffer size</param>
        /// <remarks>
        /// Notification is called when another datagram was received from some endpoint
        /// We override this method in order to let it print the data it transmits, please call "base.OnReceived" in your overrided function
        /// </remarks>
        protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            // Need at least 2 bytes
            if (size < 2 && size > 2048)
            {
                return;
            }
            byte[] temp = new byte[(int)size];
            Array.Copy(buffer, 0, temp, 0, (int)size);
            if (LogWriter.Log.DebugSockets)
                LogWriter.Log.Write(LogLevel.Debug, "{0}[Recv] UDP data: {1}", ServerName, Encoding.UTF8.GetString(buffer, 0, (int)size));

            OnReceived(endpoint, temp);
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

        public virtual void UnknownDataRecived(byte[] text)
        {
            string buffer = Encoding.UTF8.GetString(text, 0, text.Length);
            string errorMsg = string.Format("[unknow] {0}", buffer);
            ToLog(errorMsg);
        }
    }
}
