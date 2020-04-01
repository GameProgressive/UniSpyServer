using GameSpyLib.Common;
using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using NetCoreServer;
using Serilog.Events;
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
        public EndPoint Remote;
        public TemplateTcpSession(TemplateTcpServer server) : base(server)
        {
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

            ToLog(LogEventLevel.Debug, $"[Send] TCP data: { StringExtensions.ReplaceUnreadableCharToHex(buffer, 0, (int)size)}");

            return base.SendAsync(buffer, offset, size);
        }
        protected long BaseSend(byte[] buffer, long offset, long size)
        {
            return base.Send(buffer, offset, size);
        }
        protected bool BaseSendAsync(byte[] buffer, long offset, long size)
        {
            return base.SendAsync(buffer, offset, size);
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
            ToLog(LogEventLevel.Debug, $"[Send] TCP data: {StringExtensions.ReplaceUnreadableCharToHex(buffer, 0, (int)size)}");

            return base.Send(buffer, offset, size);
        }
        /// <summary>
        /// Our method to receive message and print in the console
        /// </summary>
        /// <param name="recv">message we recieved</param>
        protected virtual void OnReceived(string message) { }

        protected virtual void OnReceived(byte[] buffer)
        {
            OnReceived(Encoding.ASCII.GetString(buffer));
        }
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
                ToLog(LogEventLevel.Error, "[Spam] client spam we ignored!");
                return;
            }

            ToLog(LogEventLevel.Debug, $"[Recv] TCP data: {StringExtensions.ReplaceUnreadableCharToHex(buffer, 0, (int)size)}");

            byte[] tempBuffer = new byte[size];
            Array.Copy(buffer, 0, tempBuffer, 0, size);
            OnReceived(tempBuffer);
        }

        protected override void OnConnected()
        {
            Remote = Socket.RemoteEndPoint;
            ToLog(LogEventLevel.Information, $"[Conn] ID:{Id} IP:{Remote}");
            base.OnConnected();
        }
        protected override void OnDisconnected()
        {
            //We create our own RemoteEndPoint because when client disconnect, the session socket will dispose immidiatly
            ToLog(LogEventLevel.Information, $"[Disc] ID:{Id} IP:{Remote}");
            base.OnDisconnected();
        }

        public virtual void ToLog(LogEventLevel level, string text)
        {
            LogWriter.ToLog(level, $"[{ ServerManagerBase.ServerName}]" + text);
        }

        public virtual void UnKnownDataReceived(byte[] text)
        {
            UnknownDataReceived(Encoding.ASCII.GetString(text));
        }

        public virtual void UnknownDataReceived(string text)
        {
            ToLog(LogEventLevel.Error, $"[Unknow] {text}");
        }

        public virtual void UnknownDataReceived(Dictionary<string, string> recv)
        {
            GameSpyUtils.PrintReceivedGPDictToLogger(recv);
        }

        public virtual void LogPlainText(string data)
        {
            ToLog(LogEventLevel.Information, $@"[Plain] {data}");
        }

        public virtual void LogPlainText(byte[] data)
        {
            LogPlainText(Encoding.ASCII.GetString(data));
        }
    }
}

