using System.Collections.Generic;
using System.Net;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Database.DatabaseModel;
using UniSpy.Server.Core.Network.Tcp.Server;

namespace UniSpy.Server.ServerBrowser.Application
{
    public sealed class Server : ServerBase
    {
        public static readonly Dictionary<string, List<Grouplist>> PeerGroupList = StorageOperation.Persistance.GetAllGroupList();
        static Server()
        {
            _name = "ServerBrowser";
        }
        public Server()
        {
        }

        public Server(IConnectionManager manager) : base(manager)
        {
        }

        public override void Start()
        {
            base.Start();
            StorageOperation.HeartbeatChannel.StartSubscribe();
        }
        protected override IClient CreateClient(IConnection connection) => new Client(connection, this);

        protected override IConnectionManager CreateConnectionManager(IPEndPoint endPoint) => new TcpConnectionManager(endPoint);
    }
}