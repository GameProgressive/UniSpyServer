using System;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using GameSpyLib.Logging;
using TcpClient = NetCoreServer.TcpClient;
namespace GameSpyLib.Network
{
    public class TemplateTcpBridge : TcpClient
    {
        private string _serverName;
        public TemplateTcpBridge(string serverName, string hostname, int port) : base(hostname, port)
        {
            _serverName = serverName;
        }

        public void DisconnectAndStop()
        {
            _stop = true;
            DisconnectAsync();
            while (IsConnected)
                Thread.Yield();
        }

        protected override void OnConnected()
        {
            if (LogWriter.Log.DebugSockets)
                ToLog(LogLevel.Debug, $"[Conn] RetroSpy Server Connected!");
        }

        protected override void OnDisconnected()
        {
            if (LogWriter.Log.DebugSockets)
                ToLog(LogLevel.Debug, $"[Disc] RetroSpy Server Connected!");

            // Wait for a while...
            Thread.Sleep(1000);

            // Try to connect again
            if (!_stop)
                ConnectAsync();
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {

            if (LogWriter.Log.DebugSockets)
                ToLog(LogLevel.Debug, $" [Recv] TCP data: {FormatLogMessage(buffer, 0, (int)size)}");

            byte[] tempBuffer = new byte[size];
            Array.Copy(buffer, 0, tempBuffer, 0, size);
            OnReceived(tempBuffer);
        }
        public virtual void OnReceived(byte[] message)
        {
            OnReceived(Encoding.ASCII.GetString(message));
        }
        public virtual void OnReceived(string message)
        { }


        public override bool SendAsync(byte[] buffer, long offset, long size)
        { 
            if (LogWriter.Log.DebugSockets)
               ToLog(LogLevel.Debug, $"[Send] TCP data: {FormatLogMessage(buffer, 0, (int)size)}");

            return base.SendAsync(buffer, offset, size);
        }
        public override long Send(byte[] buffer, long offset, long size)
        {
            if (LogWriter.Log.DebugSockets)
                ToLog(LogLevel.Debug, $"[Send] TCP data: {FormatLogMessage(buffer, 0, (int)size)}");

            return base.Send(buffer, offset, size);
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"{_serverName} an error happend with code {error}");
        }

        private bool _stop;
        public virtual void ToLog(string text)
        {
            ToLog(LogLevel.Info, text);
        }
        public virtual void ToLog(LogLevel level, string text)
        {
            text = _serverName + " " + text;
            LogWriter.Log.Write(text, level);
        }
        public virtual string FormatLogMessage(byte[] buffer, int index, int size)
        {
            return Regex.Replace(Encoding.ASCII.GetString(buffer, 0, size), @"[\x00-\x1F]", "?");
        }
    }
}
