using NetCoreServer;
using Serilog.Events;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;
using UniSpyLib.Logging;

namespace UniSpyLib.Network
{
    /// <summary>
    /// This is a template class that helps creating a TCP Session (formerly TCP stream)
    /// with logging functionality and ServerName, as required in the old network stack.
    /// </summary>
    public class UniSpyTCPSessionBase : TcpSession, IUniSpySession
    {
        public EndPoint RemoteEndPoint => Socket.RemoteEndPoint;
        public IPEndPoint RemoteIPEndPoint => (IPEndPoint)Socket.RemoteEndPoint;
        public new UniSpyTCPServerBase Server => (UniSpyTCPServerBase)base.Server;

        public UniSpyTCPSessionBase(UniSpyTCPServerBase server) : base(server)
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

        public override bool SendAsync(string buffer)
        {
            return base.SendAsync(buffer);
        }

        public override bool SendAsync(byte[] buffer, long offset, long size)
        {
            LogWriter.LogNetworkTraffic("Send", RemoteIPEndPoint, buffer,size);
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
            LogWriter.LogNetworkTraffic("Recv", RemoteIPEndPoint, buffer, size);

            //LogWriter.ToLog(LogEventLevel.Debug,
            //    $"[Recv] [{RemoteIPEndPoint}] {StringExtensions.ReplaceUnreadableCharToHex(buffer, 0, (int)size)}");

            byte[] tempBuffer = new byte[size];
            Array.Copy(buffer, 0, tempBuffer, 0, size);
            OnReceived(tempBuffer);
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

