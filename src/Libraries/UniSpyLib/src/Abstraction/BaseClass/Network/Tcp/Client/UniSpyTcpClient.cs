using System;
using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Factory;
using UniSpyServer.UniSpyLib.Encryption;
using UniSpyServer.UniSpyLib.Logging;
using UniSpyServer.UniSpyLib.Config;
using TcpClient = NetCoreServer.TcpClient;

namespace UniSpyServer.UniSpyLib.Network.Tcp.Client
{

    public abstract class UniSpyTcpClient : TcpClient
    {
        /// <summary>
        /// We automatic connect to remote server address
        /// </summary>
        public UniSpyTcpClient() : base
            (
                ConfigManager.Config.Servers
            .Where(s => s.ServerName == ServerFactoryBase.ServerName)
            .First().RemoteAddress
               , ConfigManager.Config.Servers
            .Where(s => s.ServerName == ServerFactoryBase.ServerName)
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
    }
}
