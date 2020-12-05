using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;
using UniSpyLib.Logging;
using NetCoreServer;
using Serilog.Events;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UniSpyLib.Network
{
    /// <summary>
    /// This is a template class that helps creating a TCP Session (formerly TCP stream)
    /// with logging functionality and ServerName, as required in the old network stack.
    /// </summary>
    public class TCPSessionBase : TcpSession, IUniSpySession
    {
        public EndPoint RemoteEndPoint { get; private set; }

        public TCPSessionBase(TCPServerBase server) : base(server)
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

        public override bool SendAsync(string text)
        {
            return base.SendAsync(text);
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

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            if (size > 4096)
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
            RemoteEndPoint = Socket.RemoteEndPoint;
            LogWriter.ToLog(LogEventLevel.Information, $"[Conn] IP:{RemoteEndPoint}");
            base.OnConnected();
        }
        protected override void OnDisconnected()
        {
            //We create our own RemoteEndPoint because when client disconnect,
            //the session socket will dispose immidiatly
            LogWriter.ToLog(LogEventLevel.Information, $"[Disc] IP:{RemoteEndPoint}");
            base.OnDisconnected();
        }

        bool IUniSpySession.BaseSendAsync(byte[] buffer)
        {
            return BaseSendAsync(buffer);
        }
        bool IUniSpySession.BaseSendAsync(string buffer)
        {
            return BaseSendAsync(buffer);
        }
    }
}

