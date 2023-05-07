using System;
using System.Linq;
using UniSpy.Server.QueryReport.V2.Abstraction.BaseClass;
using UniSpy.Server.QueryReport.V2.Contract.Request;
using UniSpy.Server.QueryReport.V2.Contract.Result;
using UniSpy.Server.Core.Logging;
using UniSpy.Server.QueryReport.Application;

namespace UniSpy.Server.QueryReport.V2.Handler.CmdHandler
{

    public sealed class EchoHandler : CmdHandlerBase
    {
        private new EchoRequest _request => (EchoRequest)base._request;
        private new EchoResult _result { get => (EchoResult)base._result; set => base._result = value; }
        public EchoHandler(Client client, EchoRequest request) : base(client, request)
        {
            _result = new EchoResult();
        }

        protected override void DataOperation()
        {
            //TODO prevent one pc create multiple game servers
            var servers = QueryReport.V2.Application.StorageOperation.Persistance.GetServerInfos((uint)_request.InstantKey);
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
            QueryReport.V2.Application.StorageOperation.Persistance.UpdateGameServer(_result.Info);
        }
    }
}
