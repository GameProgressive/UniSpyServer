using GameSpyLib.Database.DatabaseModel.MySql;
using LinqToDB;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Handler.General.RegisterCDKey
{
    public class RegisterCDKeyHandler : GPCMHandlerBase
    {
        public RegisterCDKeyHandler(Dictionary<string, string> recv) : base(recv)
        {

        }
        protected override void CheckRequest(GPCMSession session)
        {
            base.CheckRequest(session);
            if (!_recv.ContainsKey("cdkeyenc"))
                _errorCode = Enumerator.GPErrorCode.Parse;
        }
        protected override void DataBaseOperation(GPCMSession session)
        {
            using (var db = new RetrospyDB())
            {
                var result = db.Subprofiles.Where(s => s.Profileid == session.UserInfo.Profileid
                && s.Namespaceid == session.UserInfo.NamespaceID
                && s.Productid == session.UserInfo.productID);
                if (result.Count() == 0 || result.Count() > 1)
                    _errorCode = Enumerator.GPErrorCode.DatabaseError;
                using (var tran = db.BeginTransaction())
                {
                    db.Subprofiles.Where(s => s.Profileid == session.UserInfo.Profileid
                && s.Namespaceid == session.UserInfo.NamespaceID
                && s.Productid == session.UserInfo.productID).Set(s => s.Cdkeyenc, _recv["cdkeyenc"]).Update();
                    tran.Commit();
                }
            }
        }
        protected override void ConstructResponse(GPCMSession session)
        {
            _sendingBuffer = @"\rc\final\";
        }
    }
}
