using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Structure.Request;
using PresenceConnectionManager.Network;
using System.Linq;
using UniSpyLib.Abstraction.Interface;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    /// <summary>
    /// This function sets which games the local profile can be invited to.
    /// </summary>
    internal class InviteToHandler : PCMCmdHandlerBase
    {
        //_session.SendAsync(@"\pinvite\\sesskey\223\profileid\13\productid\1038\final\");
        protected new InviteToRequest _request
        {
            get { return (InviteToRequest)base._request; }
        }
        public InviteToHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            var user = PCMServer.LoggedInSession.Values.Where(
                u => u.UserInfo.ProductID == _request.ProductID
                && u.UserInfo.ProfileID == _request.ProfileID);

            if (user.Count() == 0)
            {
                return;
            }
            //TODO
            //parse user to buddy message system
        }
    }
}
