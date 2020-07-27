using GameSpyLib.Common.Entity.Interface;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Handler.Buddy.InviteTo
{
    /// <summary>
    /// This function sets which games the local profile can be invited to.
    /// </summary>
    public class InviteToHandler : PCMCommandHandlerBase
    {
        //_session.SendAsync(@"\pinvite\\sesskey\223\profileid\13\productid\1038\final\");
        public InviteToHandler(ISession client, Dictionary<string, string> recv) : base(client, recv)
        {
        }

        //public static GPCMDBQuery DBQuery = null;

        protected override void CheckRequest()
        {
            base.CheckRequest();

            if (!_recv.ContainsKey("productid") || !_recv.ContainsKey("sesskey"))
            {
                _errorCode = GPErrorCode.Parse;
            }

            if (!_recv.ContainsKey("sesskey"))
            {
                _errorCode = GPErrorCode.Parse;
            }

            if (!uint.TryParse(_recv["productid"], out _productid))
            {
                _errorCode = GPErrorCode.Parse;
            }

            if (!uint.TryParse(_recv["profileid"], out _profileid))
            {
                _errorCode = GPErrorCode.Parse;
            }
        }

        protected override void ConstructResponse()
        {
            base.ConstructResponse();
        }

        protected override void DataOperation()
        {
            var user = PCMServer.LoggedInSession.Values.Where(
                u => u.UserData.ProductID == _productid
                && u.UserData.ProfileID == _profileid);

            if (user.Count() == 0)
            {
                return;
            }
            //TODO
            //parse user to buddy message system
        }

        protected override void Response()
        {
        }
    }
}
