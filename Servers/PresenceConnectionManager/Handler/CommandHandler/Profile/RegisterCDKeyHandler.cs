using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Database.DatabaseModel.MySql;
using PresenceConnectionManager.Entity.Structure.Request.General;
using PresenceSearchPlayer.Entity.Enumerator;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Handler.CommandHandler.Profile
{
    public class RegisterCDKeyHandler : PCMCommandHandlerBase
    {
        protected RegisterCDKeyRequest _request;
        public RegisterCDKeyHandler(ISession client, Dictionary<string, string> recv) : base(client, recv)
        {
            _request = new RegisterCDKeyRequest(recv);
        }

        protected override void CheckRequest()
        {
            _errorCode = _request.Parse();
        }

        protected override void DataOperation()
        {
            using (var db = new retrospyContext())
            {
                var result = db.Subprofiles.Where(s => s.Profileid == _session.UserData.ProfileID
                && s.Namespaceid == _session.UserData.NamespaceID);
                //&& s.Productid == _session.UserInfo.ProductID);

                if (result.Count() == 0 || result.Count() > 1)
                {
                    _errorCode = GPErrorCode.DatabaseError;
                }

                db.Subprofiles.Where(s => s.Subprofileid == _session.UserData.SubProfileID)
                    .FirstOrDefault().Cdkeyenc = _request.CDKeyEnc;

                db.SaveChanges();
            }
        }

        protected override void BuildNormalResponse()
        {
            _sendingBuffer = @"\rc\\final\";
        }
    }
}
