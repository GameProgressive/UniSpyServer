using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Structure.Request.Profile;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Linq;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    internal class RegisterNickHandler : PCMCmdHandlerBase
    {
        protected new RegisterNickRequest _request => (RegisterNickRequest)base._request;
        public RegisterNickHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
        protected override void DataOperation()
        {
            try
            {
                using (var db = new unispyContext())
                {
                    db.Subprofiles.Where(s => s.Subprofileid == _session.UserInfo.BasicInfo.SubProfileID)
                        .FirstOrDefault().Uniquenick = _request.UniqueNick;
                    db.SaveChanges();
                }
            }
            catch
            {
                _result.ErrorCode = GPErrorCode.DatabaseError;
            }
        }
    }
}
