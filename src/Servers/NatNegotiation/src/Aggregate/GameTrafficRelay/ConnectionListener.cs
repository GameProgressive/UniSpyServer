using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Extension;
using UniSpy.Server.Core.Logging;
using UniSpy.Server.Core.Network.Udp.Server;
using UniSpy.Server.NatNegotiation.Handler.CmdHandler;

namespace UniSpy.Server.NatNegotiation.Aggregate.GameTrafficRelay
{
    public class ConnectionListener : IDisposable
    {
        private IConnectionManager _manager;
        public uint Cookie { get; private set; }
        public IPEndPoint ListeningEndPoint { get; private set; }
        public bool IsDisposed { get; private set; }
        private EasyTimer _timer;
        private IUdpConnection _clientConnection;
        private IUdpConnection _serverConnection;
        private List<string> _gameServerValidIPs;
        private List<string> _gameClientValidIPs;
        public ConnectionListener(IPEndPoint listeningEndPoint, uint cookie, List<string> gameServerIPs, List<string> gameClientIPs)
        {
            ListeningEndPoint = listeningEndPoint;
            _manager = new UdpConnectionManager(listeningEndPoint);
            _manager.OnInitialization += OnIntialization;
            _gameServerValidIPs = gameServerIPs;
            _gameClientValidIPs = gameClientIPs;
            Cookie = cookie;
            _timer = new EasyTimer(TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(10));
            _timer.Elapsed += (s, e) => CheckExpiredClient();
            // after create listener we start it
            Start();
            LogWriter.LogDebug($"[{ListeningEndPoint}] gamespy client listener started.");
        }
        public void Start()
        {
            _timer.Start();
            _manager.Start();
        }

        private IClient OnIntialization(IConnection connection)
        {
            lock (_clientConnection)
            {
                if (_gameServerValidIPs.Contains(connection.RemoteIPEndPoint.ToString()) && _clientConnection is null)
                {
                    _clientConnection = connection as IUdpConnection;
                    connection.OnReceive += (buffer) => OnReceived((IUdpConnection)connection, (byte[])buffer);
                    return default;
                }
            }
            lock (_serverConnection)
            {
                if (_serverConnection is null)
                {
                    if (_gameClientValidIPs.Contains(connection.RemoteIPEndPoint.ToString()) && _serverConnection is null)
                    {
                        _serverConnection = connection as IUdpConnection;
                        connection.OnReceive += (buffer) => OnReceived((IUdpConnection)connection, (byte[])buffer);
                        return default;
                    }
                }
            }
            return default;
        }
        private void CheckExpiredClient()
        {
            if (_serverConnection is null || _clientConnection is null || _timer.IsExpired)
            {
                if (!IsDisposed)
                {
                    Dispose();
                    PingHandler.ConnectionListeners.TryRemove(this.Cookie, out _);
                    LogWriter.LogDebug($"[{ListeningEndPoint}] gamespy listener shutdown.");
                    _timer.Dispose();

                }
            }
        }
        private bool CheckValidation(byte[] buffer)
        {
            var magic = new byte[] { 0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2 };
            if (!buffer.Take(6).SequenceEqual(magic))
            {
                return false;
            }
            var cookie = buffer.Skip(8).Take(4).ToArray();

            if (Cookie != BitConverter.ToInt32(cookie))
            {
                return false;
            }

            return true;
        }
        protected void OnReceived(IUdpConnection connection, byte[] buffer)
        {

            if (_clientConnection.Equals(connection))
            {
                ForwardMessage(_clientConnection, _serverConnection, buffer);
                return;
            }

            if (_serverConnection.Equals(connection))
            {
                ForwardMessage(_serverConnection, _clientConnection, buffer);
                return;
            }
        }

        public void ForwardMessage(IUdpConnection sender, IUdpConnection receiver, byte[] data)
        {
            _timer.RefreshLastActiveTime();
            receiver.Send(data);
            LogWriter.LogDebug($"[{sender}] => [{receiver}]  {StringExtensions.ConvertPrintableBytesToString(data)} [{StringExtensions.ConvertByteToHexString(data)}]");
        }

        public void Dispose()
        {
            _manager.Dispose();
        }
    }
}