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
        protected InviteToHandler(GPCMSession session, Dictionary<string, string> recv) : base(session, recv)
        {
        }
        private uint _productid;
        private uint _profileid;
        //public static GPCMDBQuery DBQuery = null;
        protected override void CheckRequest(GPCMSession session, Dictionary<string, string> recv)
        {
            base.CheckRequest(session, recv);
            if (!recv.ContainsKey("productid") || !recv.ContainsKey("sesskey"))
                _errorCode = GPErrorCode.Parse;

            if (!recv.ContainsKey("sesskey"))
                _errorCode = GPErrorCode.Parse;
            if (!uint.TryParse(recv["productid"], out _productid))
            {
                _errorCode = GPErrorCode.Parse;
            }
            if (!uint.TryParse(recv["profileid"], out _profileid))
                _errorCode = GPErrorCode.Parse;
        }

        protected override void ConstructResponse(GPCMSession session, Dictionary<string, string> recv)
        {
            base.ConstructResponse(session, recv);
        }

        protected override void DataBaseOperation(GPCMSession session, Dictionary<string, string> recv)
        {

            var user = GPCMServer.LoggedInSession.Values.Where(
                u => u.UserInfo.productID == _productid
                && u.UserInfo.Profileid == _profileid);
            if (user.Count() == 0)
                return;
            //TODO
            //parse user to buddy message system

        }

        protected override void Response(GPCMSession session, Dictionary<string, string> recv)
        {

        }
    }
}
