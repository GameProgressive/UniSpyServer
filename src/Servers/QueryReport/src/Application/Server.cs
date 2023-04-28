using System.Collections.Generic;
using System.Net;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Database.DatabaseModel;
using UniSpy.Server.Core.Network.Udp.Server;

namespace UniSpy.Server.QueryReport.Application
{
    public sealed class Server : ServerBase
    {
        /// <summary>
        /// The peer group list in memory
        /// </summary>
        public static readonly Dictionary<string, List<Grouplist>> PeerGroupList = QueryReport.V2.Application.StorageOperation.Persistance.GetAllGroupList();
        static Server()
        {
            _name = "QueryReport";
        }
        public Server() { }

        public Server(IConnectionManager manager) : base(manager) { }

        public override void Start()
        {
            base.Start();
            V2.Application.StorageOperation.NatNegChannel.Subscribe();
            // V2.Application.StorageOperation.Persistance.InitPeerRoomsInfo();
        }
        protected override IClient CreateClient(IConnection connection) => new Client(connection, this);

        protected override IConnectionManager CreateConnectionManager(IPEndPoint endPoint) => new UdpConnectionManager(endPoint);

    }
}