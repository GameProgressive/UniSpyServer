using System.Collections.Generic;
using System.Linq;
using System.Net;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Config;
using UniSpy.Server.Core.Database.DatabaseModel;
using UniSpy.Server.Core.Network.Tcp.Server;

namespace UniSpy.Server.ServerBrowser.V2.Application
{
    public sealed class Server : ServerBase
    {
        public static readonly Dictionary<string, List<Grouplist>> PeerGroupList = StorageOperation.Persistance.GetAllGroupList();
        static Server()
        {
            _name = "ServerBrowserV2";
        }
        public Server()
        {
        }

        public Server(IConnectionManager manager) : base(manager)
        {
        }
        public override void Start()
        {
            StorageOperation.HeartbeatChannel.StartSubscribe();
            base.Start();
        }
        protected override IClient CreateClient(IConnection connection) => new Client(connection, this);

        protected override IConnectionManager CreateConnectionManager(IPEndPoint endPoint) => new TcpConnectionManager(IPEndPoint.Parse("0.0.0.0:28910"));
    }
}