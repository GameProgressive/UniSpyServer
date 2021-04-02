using Serilog.Events;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Encryption;
using UniSpyLib.Extensions;
using UniSpyLib.Logging;
using UniSpyLib.UniSpyConfig;
using TcpClient = NetCoreServer.TcpClient;

namespace UniSpyLib.Network
{

    public abstract class TCPClientBase : TcpClient
    {
        /// <summary>
        /// We automatic connect to remote server address
        /// </summary>
        public TCPClientBase() : base
            (
                ConfigManager.Config.Servers
            .Where(s => s.ServerName == UniSpyServerFactoryBase.ServerName)
            .First().RemoteAddress
               , ConfigManager.Config.Servers
            .Where(s => s.ServerName == UniSpyServerFactoryBase.ServerName)
            .First().RemotePort
            )
        {
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            LogWriter.LogNetworkTraffic("Recv", Endpoint, buffer, size);
            byte[] tempBuffer = new byte[size];
            Array.Copy(buffer, 0, tempBuffer, 0, size);
            OnReceived(tempBuffer);
        }

        protected virtual void OnReceived(byte[] buffer)
        {
            OnReceived(UniSpyEncoding.GetString(buffer));
        }

        protected virtual void OnReceived(string buffer) { }

        public override bool SendAsync(byte[] buffer, long offset, long size)
        {
            LogWriter.LogNetworkTraffic("Send", Endpoint, buffer, size);
            return base.SendAsync(buffer, offset, size);
        }

        public override long Send(byte[] buffer, long offset, long size)
        {
            LogWriter.LogNetworkTraffic("Send", Endpoint, buffer, size);
            return base.Send(buffer, offset, size);
        }

        protected override void OnError(SocketError error)
        {
            LogWriter.ToLog(LogEventLevel.Error, error.ToString());
        }
    }
}
