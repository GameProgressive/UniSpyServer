using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpy.Server.PresenceConnectionManager.Application;
using UniSpy.Server.PresenceConnectionManager.Contract.Request;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.PresenceConnectionManager.Handler.CmdHandler.Buddy
{
    /// <summary>
    /// This function sets which games the local profile can be invited to.
    /// </summary>
    public sealed class InviteToHandler : LoggedInCmdHandlerBase
    {
        //_connection.SendAsync(@"\pinvite\\sesskey\223\profileid\13\productid\1038\final\");
        private new InviteToRequest _request => (InviteToRequest)base._request;
        public InviteToHandler(IClient client, IRequest request) : base(client, request)
        {
        }

        protected override void DataOperation()
        {
            var client = ClientManager.GetClient(_request.ProfileId, _request.ProductId);

            //user is offline
            if (client is null)
            {
                return;
            }
            else
            {

            }
            //TODO
            //parse user to buddy message system
        }
    }
}
