using System;
using System.Linq;
using UniSpyServer.Servers.QueryReport.V2.Abstraction.BaseClass;
using UniSpyServer.Servers.QueryReport.V2.Application;
using UniSpyServer.Servers.QueryReport.V2.Entity.Exception;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.QueryReport.V2.Handler.CmdHandler
{

    public sealed class KeepAliveHandler : CmdHandlerBase
    {
        public KeepAliveHandler(IClient client, IRequest request) : base(client, request)
        {
        }
        protected override void DataOperation()
        {
            var result = StorageOperation.Persistance.GetServerInfos((uint)_request.InstantKey);
            if (result.Count != 1)
            {
                throw new QRException("No server or multiple servers found in redis, please make sure there is only one server.");
            }

            var gameServer = result.First();
            gameServer.LastPacketReceivedTime = DateTime.Now;
            StorageOperation.Persistance.UpdateGameServer(gameServer);
        }
    }
}
