using System.Linq;
using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request;
using UniSpyServer.Servers.PresenceConnectionManager.Structure.Data;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.CmdHandler.Buddy
{
    /// <summary>
    /// This function sets which games the local profile can be invited to.
    /// </summary>
    public sealed class InviteToHandler : CmdHandlerBase
    {
        //_connection.SendAsync(@"\pinvite\\sesskey\223\profileid\13\productid\1038\final\");
        private new InviteToRequest _request => (InviteToRequest)base._request;
        public InviteToHandler(IClient client, IRequest request) : base(client, request)
        {
        }

        protected override void DataOperation()
        {
            var connection = Client.ClientPool.Values.FirstOrDefault(
                c => ((ClientInfo)c.Info).SubProfileInfo.ProductId == _request.ProductID
                && ((ClientInfo)c.Info).SubProfileInfo.ProfileId == _request.ProfileId);

            //user is offline
            if (connection is null)
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
