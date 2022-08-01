using System;
using System.Linq;
using UniSpyServer.Servers.QueryReport.Abstraction.BaseClass;

using UniSpyServer.Servers.QueryReport.Entity.Enumerate;
using UniSpyServer.Servers.QueryReport.Entity.Exception;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.QueryReport.Handler.CmdHandler
{
    
    public sealed class KeepAliveHandler : CmdHandlerBase
    {
        public KeepAliveHandler(IClient client, IRequest request) : base(client, request)
        {
        }
        protected override void DataOperation()
        {
            var result = _redisClient.Context.Where(x => x.InstantKey == _request.InstantKey).ToList();
            if (result.Count != 1)
            {
                throw new QRException("No server or multiple servers found in redis, please make sure there is only one server.");
            }

            var gameServer = result.First();
            gameServer.LastPacketReceivedTime = DateTime.Now;
            _redisClient.SetValue(gameServer);
        }
    }
}
