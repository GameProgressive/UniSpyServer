using System;
using System.Linq;
using UniSpy.Server.QueryReport.V2.Abstraction.BaseClass;
using UniSpy.Server.QueryReport.V2.Application;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.QueryReport.Application;
using UniSpy.Server.QueryReport.V2.Contract.Request;

namespace UniSpy.Server.QueryReport.V2.Handler.CmdHandler
{

    public sealed class KeepAliveHandler : CmdHandlerBase
    {
        public KeepAliveHandler(Client client, KeepAliveRequest request) : base(client, request)
        {
        }
        protected override void DataOperation()
        {
            var result = StorageOperation.Persistance.GetServerInfos((uint)_request.InstantKey);
            if (result.Count != 1)
            {
                throw new QueryReport.Exception("No server or multiple servers found in redis, please make sure there is only one server.");
            }

            var gameServer = result.First();
            gameServer.LastPacketReceivedTime = DateTime.Now;
            StorageOperation.Persistance.UpdateGameServer(gameServer);
        }
    }
}
