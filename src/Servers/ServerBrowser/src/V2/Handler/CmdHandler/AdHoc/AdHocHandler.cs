using System.Threading.Tasks;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Logging;
using UniSpy.Server.QueryReport.V2.Aggregate.Redis.GameServer;
using UniSpy.Server.ServerBrowser.V2.Application;
using UniSpy.Server.ServerBrowser.V2.Contract.Response;
using UniSpy.Server.ServerBrowser.V2.Contract.Response.AdHoc;
using UniSpy.Server.ServerBrowser.V2.Contract.Result;
using UniSpy.Server.ServerBrowser.V2.Enumerate;

namespace UniSpy.Server.ServerBrowser.V2.Handler.CmdHandler.AdHoc
{
    public class AdHocHandler : IHandler
    {
        public GameServerInfo _message;
        public AdHocHandler(GameServerInfo message)
        {
            LogWriter.LogCurrentClass(this);
            _message = message;
        }

        public void Handle()
        {
            IResponse response = null;
            var result = new AdHocResult() { GameServerInfo = _message };
            switch (_message.ServerStatus)
            {
                case QueryReport.V2.Enumerate.GameServerStatus.Normal:
                case QueryReport.V2.Enumerate.GameServerStatus.Update:
                case QueryReport.V2.Enumerate.GameServerStatus.Playing:
                    response = new UpdateServerInfoResponse(result);
                    break;
                case QueryReport.V2.Enumerate.GameServerStatus.Shutdown:
                    response = new DeleteServerInfoResponse(result);
                    break;
            }

            var clients = ClientManager.GetClient(_message.GameName);
            Parallel.ForEach(clients, client =>
            {
                if (client.Info.GameName == _message.GameName
                && client.Crypto is not null
                && (client.Info.SearchType == ServerListUpdateOption.ServerMainList
                || client.Info.SearchType == ServerListUpdateOption.P2PServerMainList))
                {
                    client.LogInfo($"Sending AdHoc message {_message.ServerStatus} to client");
                    client.Send(response);
                }
            });
        }
    }
}