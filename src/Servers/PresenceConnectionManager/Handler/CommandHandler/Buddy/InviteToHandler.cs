using GameSpyLib.Common.Entity.Interface;
using PresenceConnectionManager.Entity.Structure.Request.Buddy;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Handler.CommandHandler.Buddy
{
    /// <summary>
    /// This function sets which games the local profile can be invited to.
    /// </summary>
    public class InviteToHandler : PCMCommandHandlerBase
    {
        //_session.SendAsync(@"\pinvite\\sesskey\223\profileid\13\productid\1038\final\");
        protected InviteToRequest _request;
        public InviteToHandler(ISession client, Dictionary<string, string> recv) : base(client, recv)
        {
            _request = new InviteToRequest(recv);
        }

        protected override void CheckRequest()
        {
            _errorCode = _request.Parse();
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
