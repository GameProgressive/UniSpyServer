using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;
using PresenceConnectionManager.Entity.Structure.Request.Profile;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Abstraction.BaseClass.Profile
{
    public class RegisterNickHandler : PCMCommandHandlerBase
    {
        protected RegisterNickRequest _request;
        public RegisterNickHandler(ISession client, Dictionary<string, string> recv) : base(client, recv)
        {
            _request = new RegisterNickRequest(recv);
        }

        protected override void CheckRequest()
        {
            _errorCode = _request.Parse();
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
                _errorCode = GPError.DatabaseError;
            }
        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
            _sendingBuffer = $@"\rn\\id\{_request.OperationID}\final\";
        }
    }
}
