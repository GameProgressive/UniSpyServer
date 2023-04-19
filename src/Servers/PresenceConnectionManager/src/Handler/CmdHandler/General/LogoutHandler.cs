using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.PresenceConnectionManager.Application;
using UniSpy.Server.PresenceConnectionManager.Contract.Request;

namespace UniSpy.Server.PresenceConnectionManager.Handler.CmdHandler.General
{

    public sealed class LogoutHandler : CmdHandlerBase
    {
        public LogoutHandler(Client client, LogoutRequest request) : base(client, request)
        {
        }

        protected override void DataOperation()
        {
            _client.Connection.Disconnect();
        }
    }
}
