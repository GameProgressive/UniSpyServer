using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using GameSpyLib.MiscMethod;
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
    /// This is a template class that helps creating a TCP Session (formerly TCP stream)
    /// with logging functionality and ServerName, as required in the old network stack.
    /// </summary>
    public class TemplateTcpSession : TcpSession
    {
        private EndPoint _endPoint;
        public TemplateTcpSession(TemplateTcpServer server) : base(server)
        {
        }

        /// <summary>
        /// Handle error notification
        /// </summary>
        /// <param name="error">Socket error code</param>
        protected override void OnError(SocketError error)
        {
            LogWriter.ToLog(LogEventLevel.Error, error.ToString());
        }

        public bool BaseSendAsync(string buffer)
        {
            return BaseSendAsync(Encoding.ASCII.GetBytes(buffer));
        }

        public bool BaseSendAsync(byte[] buffer)
        {
            return BaseSendAsync(buffer, 0, buffer.Length);
        }

        protected bool BaseSendAsync(byte[] buffer, long offset, long size)
        {
            return base.SendAsync(buffer, offset, size);
        }

        public override bool SendAsync(byte[] buffer, long offset, long size)
        {

            LogWriter.ToLog(LogEventLevel.Debug,
                $"[Send] { StringExtensions.ReplaceUnreadableCharToHex(buffer, 0, (int)size)}");

            return base.SendAsync(buffer, offset, size);
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
                LogWriter.ToLog(LogEventLevel.Error, "[Spam] client spam we ignored!");
                return;
            }

            LogWriter.ToLog(LogEventLevel.Debug,
                $"[Recv] {StringExtensions.ReplaceUnreadableCharToHex(buffer, 0, (int)size)}");

            byte[] tempBuffer = new byte[size];
            Array.Copy(buffer, 0, tempBuffer, 0, size);
            OnReceived(tempBuffer);
        }

        protected override void OnConnected()
        {
            _endPoint = Socket.RemoteEndPoint;
            LogWriter.ToLog(LogEventLevel.Information, $"[Conn] IP:{_endPoint}");
            base.OnConnected();
        }
        protected override void OnDisconnected()
        {
            //We create our own RemoteEndPoint because when client disconnect, the session socket will dispose immidiatly
            LogWriter.ToLog(LogEventLevel.Information, $"[Disc] IP:{_endPoint}");
            base.OnDisconnected();
        }

     
        public virtual void UnKnownDataReceived(byte[] text)
        {
            UnknownDataReceived(Encoding.ASCII.GetString(text));
        }

        public virtual void UnknownDataReceived(string text)
        {
            LogWriter.ToLog(LogEventLevel.Error, $"[Unknow] {text}");
        }

        public virtual void UnknownDataReceived(Dictionary<string, string> recv)
        {
            GameSpyUtils.PrintReceivedGPDictToLogger(recv);
        }

        public virtual void LogPlainText(string data)
        {
            LogWriter.ToLog(LogEventLevel.Information, $@"[Plain] {data}");
        }

        public virtual void LogPlainText(byte[] data)
        {
            LogPlainText(StringExtensions.ReplaceUnreadableCharToHex(data));
        }
    }
}

