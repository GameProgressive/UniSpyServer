using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using UniSpyServer.Servers.GameTrafficRelay.Controller;
using UniSpyServer.Servers.GameTrafficRelay.Interface;
using UniSpyServer.UniSpyLib.Extensions;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.GameTrafficRelay.Entity.Structure
{
    public class ConnectionListener : NetCoreServer.UdpServer, IConnectionListener
    {
        public uint Cookie { get; private set; }
        public IPEndPoint ListeningEndPoint => (IPEndPoint)Endpoint;
        /// <summary>
        /// The other gamespy client that message need forward to
        /// </summary>
        public IConnectionListener ForwardTargetListener { get; set; }
        public IPEndPoint GameSpyClientIPEndPoint { get; private set; }
        public DateTime LastPacketReceivedTime { get; private set; }
        public TimeSpan ConnectionExistedTime => DateTime.Now - LastPacketReceivedTime;
        private TimeSpan _expireTimeInterval;
        private System.Timers.Timer _timer;
        public ConnectionListener(IPEndPoint listeningEndPoint, uint cookie) : base(listeningEndPoint.Address, listeningEndPoint.Port)
        {
            // after create listener we start it
            Cookie = cookie;
            Start();
            LastPacketReceivedTime = DateTime.Now;
            // we set expire time to 2 minutes
            _expireTimeInterval = new TimeSpan(0, 2, 0);
            _timer = new System.Timers.Timer
            {
                Enabled = true,
                Interval = 60000,
                AutoReset = true
            };//10000
            _timer.Start();
            _timer.Elapsed += (s, e) => CheckExpiredClient();
            LogWriter.Debug($"[{ListeningEndPoint}] gamespy client listener started.");
        }
        protected override void OnStarted() => ReceiveAsync();
        private void CheckExpiredClient()
        {
            // we calculate the interval between last packe and current time
            if (ConnectionExistedTime > _expireTimeInterval)
            {
                ((IConnectionListener)this).Dispose();
            }
        }
        protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            var message = buffer.Skip((int)offset).Take((int)size).ToArray();
            OnReceived(endpoint, message);
        }
        protected void OnReceived(EndPoint endPoint, byte[] buffer)
        {
            // we only accept the gamespy client message

            if (GameSpyClientIPEndPoint is null)
            {
                GameSpyClientIPEndPoint = (IPEndPoint)endPoint;
            }

            ThreadPool.QueueUserWorkItem(o => { try { ReceiveAsync(); } catch { } });
            LogWriter.Debug($"[{GameSpyClientIPEndPoint}] [recv] {StringExtensions.ConvertPrintableCharToString(buffer)} [{StringExtensions.ConvertByteToHexString(buffer)}]");
            ForwardTargetListener.ForwardMessage(buffer);
            LastPacketReceivedTime = DateTime.Now;
        }

        public void ForwardMessage(byte[] data)
        {
            // we must send the every message to both client
            var retryCount = 0;
            // we wait for clients connect 5 sec
            if (GameSpyClientIPEndPoint is null)
            {
                LogWriter.Debug($"[{ListeningEndPoint}] is waiting for gamespy client to connect.");
                Thread.Sleep(5000);
                retryCount++;
            }

            // after waiting for 10 sec client still not connecting we just dispose it
            if (GameSpyClientIPEndPoint is null)
            {
                ForwardTargetListener?.Dispose();
                if (NatNegotiationController.ConnectionPairs.TryGetValue(Cookie, out _))
                {
                    NatNegotiationController.ConnectionPairs.Remove(Cookie);
                }
                ((IConnectionListener)this).Dispose();
                return;
            }
            LogWriter.Debug($"[{ListeningEndPoint}] => [{GameSpyClientIPEndPoint}]  {StringExtensions.ConvertPrintableCharToString(data)} [{StringExtensions.ConvertByteToHexString(data)}]");
            SendAsync(GameSpyClientIPEndPoint, data);
        }

        void IConnectionListener.Dispose()
        {
            LogWriter.Debug($"[{ListeningEndPoint}] gamespy client listener stoped");
            Dispose();
        }
    }
}