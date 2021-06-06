using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Structure.Request;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Linq;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    internal class RegisterCDKeyHandler : PCMCmdHandlerBase
    {
        protected new RegisterCDKeyRequest _request => (RegisterCDKeyRequest)base._request;
        public RegisterCDKeyHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            using (var db = new unispyContext())
            {
                var result = db.Subprofiles.Where(s => s.Profileid == _session.UserInfo.BasicInfo.ProfileID
                && s.Namespaceid == _session.UserInfo.BasicInfo.NamespaceID);
                //&& s.Productid == _session.UserInfo.ProductID);

                if (result.Count() == 0 || result.Count() > 1)
                {
                    _result.ErrorCode = GPErrorCode.DatabaseError;
                }

                db.Subprofiles.Where(s => s.Subprofileid == _session.UserInfo.BasicInfo.SubProfileID)
                    .FirstOrDefault().Cdkeyenc = _request.CDKeyEnc;

                db.SaveChanges();
            }
        }
    }
}
