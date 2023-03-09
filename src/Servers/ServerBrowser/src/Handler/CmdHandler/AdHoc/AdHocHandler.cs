using System.Threading.Tasks;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.QueryReport.Aggregate.Redis.GameServer;
using UniSpy.Server.ServerBrowser.Application;
using UniSpy.Server.ServerBrowser.Contract.Request;
using UniSpy.Server.ServerBrowser.Contract.Response;
using UniSpy.Server.ServerBrowser.Contract.Result;

namespace UniSpy.Server.ServerBrowser.Handler.CmdHandler.AdHoc
{
    public class AdHocHandler : IHandler
    {
        public GameServerInfo _message;
        public AdHocHandler(GameServerInfo message)
        {
            _message = message;
        }

        public void Handle()
        {
            var response = new ServerInfoResponse(
               null,
               new ServerInfoResult() { GameServerInfo = _message }
           );
            var clients = ClientManager.GetClient(_message.GameName);
            Parallel.ForEach(clients, client =>
            {
                if (((Client)client).Info.GameName == _message.GameName && client.Crypto is not null)
                {
                    client.Send(response);
                }
            });
        }
    }
}