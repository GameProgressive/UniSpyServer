using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.PresenceConnectionManager.Application;

namespace UniSpy.Server.PresenceConnectionManager.Handler.CmdHandler.Profile
{
    /// <summary>
    /// Update user information (email)
    /// </summary>

    public sealed class UpdateUserInfoHandler : LoggedInCmdHandlerBase
    {
        public UpdateUserInfoHandler(Client client, IRequest request) : base(client, request)
        {
            //todo find what data is belong to user info
            throw new System.NotImplementedException();
        }

        protected override void DataOperation()
        {
            throw new System.NotImplementedException();
        }
    }
}
