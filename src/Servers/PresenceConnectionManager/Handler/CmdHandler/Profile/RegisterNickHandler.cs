using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;
using PresenceConnectionManager.Entity.Structure.Request.Profile;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;
using System.Linq;
using PresenceConnectionManager.Abstraction.BaseClass;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    public class RegisterNickHandler : PCMCommandHandlerBase
    {
        protected new RegisterNickRequest _request { get { return (RegisterNickRequest)base._request; } }
        public RegisterNickHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            try
            {
                using (var db = new retrospyContext())
                {
                    db.Subprofiles.Where(s => s.Subprofileid == _session.UserData.SubProfileID)
                        .FirstOrDefault().Uniquenick = _request.UniqueNick;
                    db.SaveChanges();
                }
            }
            catch
            {
                _errorCode = GPErrorCode.DatabaseError;
            }
        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
            _sendingBuffer = $@"\rn\\id\{_request.OperationID}\final\";
        }
    }
}
