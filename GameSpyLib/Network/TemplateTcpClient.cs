using GameSpyLib.Common;
using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using Serilog.Events;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TcpClient = NetCoreServer.TcpClient;

namespace GameSpyLib.Network
{
    public class TemplateTcpClient : TcpClient
    {
        public TemplateTcpClient(string hostname, int port) : base(hostname, port)
        {
            ConnectAsync();
        }

        protected override void OnConnected()
        {
            ToLog(LogEventLevel.Information, $"[Proxy] [Conn] {Socket.RemoteEndPoint} Connected!");
        }

        protected override void OnDisconnected()
        {
            ToLog(LogEventLevel.Information, $"[Proxy] [Disc] {Socket.RemoteEndPoint} disconnected!");
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            ToLog(LogEventLevel.Debug,
                $"[Proxy] [Recv] TCP data: {StringExtensions.ReplaceUnreadableCharToHex(buffer, 0, (int)size)}");

            byte[] tempBuffer = new byte[size];
            Array.Copy(buffer, 0, tempBuffer, 0, size);
            OnReceived(tempBuffer);
        }
        public virtual void OnReceived(byte[] message)
        {
            OnReceived(Encoding.ASCII.GetString(message));
        }
        public virtual void OnReceived(string message)
        {

        }

        public override bool SendAsync(byte[] buffer, long offset, long size)
        {
            ToLog(LogEventLevel.Debug,
                $"[Proxy] [Send] TCP data: {StringExtensions.ReplaceUnreadableCharToHex(buffer, 0, (int)size)}");

            return base.SendAsync(buffer, offset, size);
        }
        public override long Send(byte[] buffer, long offset, long size)
        {
            ToLog(LogEventLevel.Debug,
                $"[Proxy] [Send] TCP data: {StringExtensions.ReplaceUnreadableCharToHex(buffer, 0, (int)size)}");
            return base.Send(buffer, offset, size);
        }

        protected override void OnError(SocketError error)
        {
            ToLog(LogEventLevel.Error, error.ToString());
        }

        public virtual void ToLog(LogEventLevel level, string text)
        {
            LogWriter.ToLog(level, $"[{ServerManagerBase.ServerName}] " + text);
        }

    }
}
