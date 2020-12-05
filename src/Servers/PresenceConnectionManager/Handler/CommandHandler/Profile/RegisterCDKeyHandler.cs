using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;
using PresenceConnectionManager.Entity.Structure.Request.General;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;
using System.Linq;
using PresenceConnectionManager.Abstraction.BaseClass;

namespace PresenceConnectionManager.Handler.CommandHandler
{
    public class RegisterCDKeyHandler : PCMCommandHandlerBase
    {
        protected new RegisterCDKeyRequest _request;
        public RegisterCDKeyHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _request = (RegisterCDKeyRequest)request;
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
