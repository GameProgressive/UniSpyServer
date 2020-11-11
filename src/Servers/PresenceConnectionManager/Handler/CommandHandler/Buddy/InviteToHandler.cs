using UniSpyLib.Abstraction.Interface;
using PresenceConnectionManager.Entity.Structure.Request.Buddy;
using System.Collections.Generic;
using System.Linq;
using PresenceConnectionManager.Network;

namespace PresenceConnectionManager.Abstraction.BaseClass.Buddy
{
    /// <summary>
    /// This function sets which games the local profile can be invited to.
    /// </summary>
    public class InviteToHandler : PCMCommandHandlerBase
    {
        //_session.SendAsync(@"\pinvite\\sesskey\223\profileid\13\productid\1038\final\");
        protected new  InviteToRequest _request;
        public InviteToHandler(ISession session, IRequest request) : base(session, request)
        {
            _request = (InviteToRequest)request;
        }

        protected override void DataOperation()
        {
            var user = PCMServer.LoggedInSession.Values.Where(
                u => u.UserData.ProductID == _request.ProductID
                && u.UserData.ProfileID == _request.ProfileID);

            if (user.Count() == 0)
            {
                return;
            }
            //TODO
            //parse user to buddy message system
        }

    }
}
