using System;
using System.Linq;
using UniSpyServer.Servers.QueryReport.V2.Abstraction.BaseClass;
using UniSpyServer.Servers.QueryReport.V2.Application;
using UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Request;
using UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.QueryReport.V2.Handler.CmdHandler
{

    public sealed class EchoHandler : CmdHandlerBase
    {
        private new EchoRequest _request => (EchoRequest)base._request;
        private new EchoResult _result { get => (EchoResult)base._result; set => base._result = value; }
        public EchoHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new EchoResult();
        }

        protected override void DataOperation()
        {
            //TODO prevent one pc create multiple game servers
            var servers = StorageOperation.Persistance.GetServerInfos((uint)_request.InstantKey);
            if (servers.Count() != 1)
            {
                _client.LogInfo("Can not find game server");
                return;
            }
            //add recive echo packet on gameserverList

            //we get the first key in matchedKeys
            _result.Info = servers.First();
            _result.Info.LastPacketReceivedTime = DateTime.Now;
            // StorageOperation.Persistance.UpdateGameServer(_result.Info);
            StorageOperation.Persistance.UpdateGameServer(_result.Info);
        }
    }
}
