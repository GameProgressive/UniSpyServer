using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Database.DatabaseModel.MySql;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Handler.General.RegisterCDKey
{
    public class RegisterCDKeyHandler :  PCMCommandHandlerBase
    {
        public RegisterCDKeyHandler(IClient client, Dictionary<string, string> recv) : base(client, recv)
        {
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();

            if (!_recv.ContainsKey("cdkeyenc"))
            {
                _errorCode = Enumerator.GPErrorCode.Parse;
            }
        }

        protected override void DataOperation()
        {
            using (var db = new retrospyContext())
            {
                var result = db.Subprofiles.Where(s => s.Profileid == _session.UserInfo.Profileid
                && s.Namespaceid == _session.UserInfo.NamespaceID
                && s.Productid == _session.UserInfo.productID);

                if (result.Count() == 0 || result.Count() > 1)
                {
                    _errorCode = Enumerator.GPErrorCode.DatabaseError;
                }

                db.Subprofiles.Where(s => s.Profileid == _session.UserInfo.Profileid
            && s.Namespaceid == _session.UserInfo.NamespaceID
            && s.Productid == _session.UserInfo.productID).FirstOrDefault()
            .Cdkeyenc = _recv["cdkeyenc"];
                db.SaveChanges();
            }
        }

        protected override void ConstructResponse()
        {
            _sendingBuffer = @"\rc\final\";
        }
    }
}
