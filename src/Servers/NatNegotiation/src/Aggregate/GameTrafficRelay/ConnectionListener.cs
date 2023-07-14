using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Extension;
using UniSpy.Server.Core.Logging;
using UniSpy.Server.Core.Network.Udp.Server;
using UniSpy.Server.NatNegotiation.Handler.CmdHandler;

namespace UniSpy.Server.NatNegotiation.Aggregate.GameTrafficRelay
{
    public class ConnectionListener : NetCoreServer.UdpServer
    {
        private IConnectionManager _manager;
        public uint Cookie { get; private set; }
        public IPEndPoint ListeningEndPoint => (IPEndPoint)Endpoint;
        private EasyTimer _timer;
        private IPAddress _gameServerAddress;
        private IPAddress _gameClientAddress;
        private IPEndPoint _gameServerEndPoint;
        private IPEndPoint _gameClientEndPoint;
        private List<string> _gameServerValidIPs;
        private List<string> _gameClientValidIPs;
        public ConnectionListener(IPEndPoint listeningEndPoint, uint cookie, List<string> gameServerIPs, List<string> gameClientIPs) : base(listeningEndPoint)
        {
            _manager = new UdpConnectionManager(listeningEndPoint);
            // _manager.OnInitialization
            // _gameServerAddress = IPEndPoint.Parse(request.GameServerIP);
            // _gameClientAddress = IPEndPoint.Parse(request.GameClientIP);
            _gameServerValidIPs = gameServerIPs;
            _gameClientValidIPs = gameClientIPs;
            Cookie = cookie;
            _timer = new EasyTimer(TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(10), CheckExpiredClient);
            // after create listener we start it
            Start();
            LogWriter.LogDebug($"[{ListeningEndPoint}] gamespy client listener started.");
        }
        private void OnInit()
        {

        }
        protected override void OnStarted() => ReceiveAsync();
        private void CheckExpiredClient()
        {
            if (_gameClientEndPoint is null || _gameServerEndPoint is null || _timer.IsExpired)
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
        protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size) => OnReceived(endpoint, buffer.Skip((int)offset).Take((int)size).ToArray());
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
        protected void OnReceived(EndPoint endPoint, byte[] buffer)
        {
            ThreadPool.QueueUserWorkItem(o => { try { ReceiveAsync(); } catch { } });

            // we only accept the gamespy client message
            if (_gameClientEndPoint is null || _gameClientEndPoint is null)
            {
                if (_gameServerValidIPs.Contains(endPoint.ToString()) && _gameServerEndPoint is null)
                {
                    _gameServerEndPoint = (IPEndPoint)endPoint;
                }
                else if (_gameClientValidIPs.Contains(endPoint.ToString()) && _gameClientEndPoint is null && !_gameClientAddress.Equals((IPEndPoint)endPoint))
                {
                    _gameClientEndPoint = (IPEndPoint)endPoint;
                }
                else
                {
                    //ignore
                }
                LogWriter.LogDebug($"[{endPoint}] [recv] {StringExtensions.ConvertPrintableBytesToString(buffer)} [{StringExtensions.ConvertByteToHexString(buffer)}]");
            }
            else
            {
                if (_gameServerEndPoint.Equals((IPEndPoint)endPoint))
                {
                    ForwardMessage(_gameServerEndPoint, _gameClientEndPoint, buffer);
                }
                else if (_gameClientEndPoint.Equals((IPEndPoint)endPoint))
                {
                    ForwardMessage(_gameClientEndPoint, _gameServerEndPoint, buffer);
                }
                else
                {
                    //ignore
                }
            }
        }

        public void ForwardMessage(IPEndPoint sender, IPEndPoint receiver, byte[] data)
        {
            _timer.RefreshLastActiveTime();
            SendAsync(receiver, data);
            LogWriter.LogDebug($"[{sender}] => [{receiver}]  {StringExtensions.ConvertPrintableBytesToString(data)} [{StringExtensions.ConvertByteToHexString(data)}]");
        }
    }
}