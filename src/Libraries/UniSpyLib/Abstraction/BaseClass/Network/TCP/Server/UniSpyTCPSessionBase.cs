using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using NetCoreServer;
using Serilog.Events;
using UniSpyLib.Abstraction.Interface;
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
        protected override void OnError(SocketError error) => LogWriter.ToLog(LogEventLevel.Error, error.ToString());
        bool IUniSpySession.BaseSendAsync(byte[] buffer) => BaseSendAsync(buffer);
        bool IUniSpySession.BaseSendAsync(string buffer) => BaseSendAsync(buffer);
        public bool BaseSendAsync(string buffer) => BaseSendAsync(Encoding.ASCII.GetBytes(buffer));
        public bool BaseSendAsync(byte[] buffer) => BaseSendAsync(buffer, 0, buffer.Length);
        protected bool BaseSendAsync(byte[] buffer, long offset, long size) => base.SendAsync(buffer, offset, size);
        public override bool SendAsync(string buffer) => base.SendAsync(buffer);

        protected virtual void OnReceived(string message) { }
        protected virtual void OnReceived(byte[] buffer) => OnReceived(Encoding.ASCII.GetString(buffer));
    }
}

