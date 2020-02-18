using PresenceConnectionManager.Enumerator;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Handler.Buddy.InviteTo
{
    /// <summary>
    /// This function sets which games the local profile can be invited to.
    /// </summary>
    public class InviteToHandler : GPCMHandlerBase
    {
        //session.SendAsync(@"\pinvite\\sesskey\223\profileid\13\productid\1038\final\");
        protected InviteToHandler(Dictionary<string, string> recv) : base(recv)
        {
        }
        private uint _productid;
        private uint _profileid;
        //public static GPCMDBQuery DBQuery = null;
        protected override void CheckRequest(GPCMSession session)
        {
            base.CheckRequest(session);
            if (!_recv.ContainsKey("productid") || !_recv.ContainsKey("sesskey"))
                _errorCode = GPErrorCode.Parse;

            if (!_recv.ContainsKey("sesskey"))
                _errorCode = GPErrorCode.Parse;
            if (!uint.TryParse(_recv["productid"], out _productid))
            {
                _errorCode = GPErrorCode.Parse;
            }
            if (!uint.TryParse(_recv["profileid"], out _profileid))
                _errorCode = GPErrorCode.Parse;
        }

        protected override void ConstructResponse(GPCMSession session)
        {
            base.ConstructResponse(session);
        }

        protected override void DataBaseOperation(GPCMSession session)
        {

            var user = GPCMServer.LoggedInSession.Values.Where(
                u => u.UserInfo.productID == _productid
                && u.UserInfo.Profileid == _profileid);
            if (user.Count() == 0)
                return;
            //TODO
            //parse user to buddy message system

        }

        protected override void Response(GPCMSession session)
        {

        }
    }
}
