using System;
using System.Linq;
using System.Net;
using System.Threading;
using UniSpy.Server.Core.Extension;
using UniSpy.Server.Core.Logging;
using UniSpy.Server.GameTrafficRelay.Controller;

namespace UniSpy.Server.GameTrafficRelay.Aggregate
{
    public class ConnectionListener : NetCoreServer.UdpServer
    {
        public uint Cookie { get; private set; }
        public IPEndPoint ListeningEndPoint => (IPEndPoint)Endpoint;
        private EasyTimer _timer;
        private IPAddress _gameServerAddress;
        private IPAddress _gameClientAddress;
        private IPEndPoint _gameServerEndPoint;
        private IPEndPoint _gameClientEndPoint;
        public ConnectionListener(IPEndPoint listeningEndPoint, uint cookie, IPAddress gameServerAddr, IPAddress gameClientAddr) : base(listeningEndPoint)
        {
            _gameServerAddress = gameServerAddr;
            _gameClientAddress = gameClientAddr;
            Cookie = cookie;
            _timer = new EasyTimer(TimeSpan.FromMinutes(10), TimeSpan.FromSeconds(10), CheckExpiredClient);
            // after create listener we start it
            Start();
            LogWriter.LogDebug($"[{ListeningEndPoint}] gamespy client listener started.");
        }
        protected override void OnStarted() => ReceiveAsync();
        private void CheckExpiredClient()
        {
            if (_gameServerEndPoint is null || _gameClientEndPoint is null)
            {
                if (!IsDisposed)
                {
                    Dispose();
                    NatNegotiationController.ConnectionListeners.TryRemove(this.Cookie, out _);
                    LogWriter.LogDebug($"[{ListeningEndPoint}] gamespy listener shutdown.");
                }
            }
        }
        protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            var message = buffer.Skip((int)offset).Take((int)size).ToArray();
            OnReceived(endpoint, message);
        }
        protected void OnReceived(EndPoint endPoint, byte[] buffer)
        {
            ThreadPool.QueueUserWorkItem(o => { try { ReceiveAsync(); } catch { } });
            LogWriter.LogDebug($"[{endPoint}] [recv] {StringExtensions.ConvertPrintableBytesToString(buffer)} [{StringExtensions.ConvertByteToHexString(buffer)}]");
            // we only accept the gamespy client message
            if (_gameClientEndPoint is null || _gameClientEndPoint is null)
            {
                if (_gameServerAddress.Equals(((IPEndPoint)endPoint).Address) && _gameServerEndPoint is null)
                {
                    _gameServerEndPoint = (IPEndPoint)endPoint;
                }
                else if (_gameClientAddress.Equals(((IPEndPoint)endPoint).Address) && _gameClientEndPoint is null && !_gameClientAddress.Equals((IPEndPoint)endPoint))
                {
                    _gameClientEndPoint = (IPEndPoint)endPoint;
                }
                else
                {
                    //ignore
                }
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