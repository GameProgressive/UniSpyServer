using System.Threading.Tasks;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Logging;
using UniSpy.Server.QueryReport.Aggregate.Redis.GameServer;
using UniSpy.Server.ServerBrowser.Application;
using UniSpy.Server.ServerBrowser.Contract.Response;
using UniSpy.Server.ServerBrowser.Contract.Response.AdHoc;
using UniSpy.Server.ServerBrowser.Contract.Result;

namespace UniSpy.Server.ServerBrowser.Handler.CmdHandler.AdHoc
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
                case QueryReport.Enumerate.GameServerStatus.Normal:
                case QueryReport.Enumerate.GameServerStatus.Update:
                case QueryReport.Enumerate.GameServerStatus.Playing:
                    response = new UpdateServerInfoResponse(result);
                    break;
                case QueryReport.Enumerate.GameServerStatus.Shutdown:
                    response = new DeleteServerInfoResponse(result);
                    break;
            }

            var clients = ClientManager.GetClient(_message.GameName);
            Parallel.ForEach(clients, client =>
            {
                if (((Client)client).Info.GameName == _message.GameName && client.Crypto is not null)
                {
                    client.LogInfo($"Sending AdHoc message {_message.ServerStatus} to client");
                    client.Send(response);
                }
            });
        }
    }
}