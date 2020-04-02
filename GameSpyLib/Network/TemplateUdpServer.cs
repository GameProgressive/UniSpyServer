using GameSpyLib.Common;
using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using NetCoreServer;
using Serilog.Events;
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
        /// Initialize UDP server with a given IP address and port number
        /// </summary>
        /// <param name="serverName">The name of the server that will be started</param>
        /// <param name="address">IP address</param>
        /// <param name="port">Port number</param>
        public TemplateUdpServer(IPEndPoint endpoint) : base(endpoint)
        {
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
        public TemplateUdpServer(IPAddress address, int port) : base(address, port)
        {
            Start();
        }

        /// <summary>
        /// Handle error notification
        /// </summary>
        /// <param name="error">Socket error code</param>
        protected override void OnError(SocketError error)
        {
            ToLog(LogEventLevel.Error, error.ToString());
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
            ToLog(LogEventLevel.Debug, $"[Send] {StringExtensions.ReplaceUnreadableCharToHex(buffer, 0, (int)size)}");

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
            ToLog(LogEventLevel.Debug, $"[Send] {StringExtensions.ReplaceUnreadableCharToHex(buffer, 0, (int)size)}");

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

            ToLog(LogEventLevel.Debug, $"[Recv] {StringExtensions.ReplaceUnreadableCharToHex(buffer, 0, (int)size)}");
            //even if we did not response we keep receive message
            ReceiveAsync();

            OnReceived(endpoint, temp);
        }

        public virtual void ToLog(string message)
        {
            ToLog(LogEventLevel.Information, message);
        }

        public virtual void ToLog(LogEventLevel level, string text)
        {
            LogWriter.ToLog(level, $"[{ServerManagerBase.ServerName}] " + text);
        }

        public virtual void UnknownDataRecived(byte[] text)
        {
            string buffer = Encoding.ASCII.GetString(text, 0, text.Length);
            ToLog(LogEventLevel.Error, "[Unknown] " + buffer);
        }
    }
}
