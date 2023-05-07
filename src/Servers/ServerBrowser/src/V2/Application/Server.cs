using System.Net;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Logging;
using UniSpy.Server.Core.Network.Tcp.Server;

namespace UniSpy.Server.ServerBrowser.V2.Application
{
    public sealed class Server : ServerBase
    {
        static Server()
        {
            _name = "ServerBrowserV2";
        }
        public Server() { }

        public Server(IConnectionManager manager) : base(manager) { }
        public override void Start()
        {
            QueryReport.V2.Application.StorageOperation.HeartbeatChannel.Subscribe();
            QueryReport.V2.Application.StorageOperation.HeartbeatChannel.OnReceived += ReceivedMessage;
            base.Start();
        }
        public void ReceivedMessage(QueryReport.V2.Aggregate.Redis.GameServer.GameServerInfo message)
        {
            LogWriter.LogInfo($"Received game server message from QR:{message.ServerID}");
            var handler = new Handler.CmdHandler.AdHoc.AdHocHandler(message);
            handler.Handle();
        }
        protected override IClient CreateClient(IConnection connection) => new Client(connection, this);

        protected override IConnectionManager CreateConnectionManager(IPEndPoint endPoint) => new TcpConnectionManager(endPoint);
    }
}