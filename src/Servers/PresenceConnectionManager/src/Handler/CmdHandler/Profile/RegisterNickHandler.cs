using UniSpyServer.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.PresenceConnectionManager.Entity.Contract;
using UniSpyServer.PresenceConnectionManager.Entity.Structure.Request.Profile;
using UniSpyServer.PresenceSearchPlayer.Entity.Exception.General;
using System;
using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel.MySql;

namespace UniSpyServer.PresenceConnectionManager.Handler.CmdHandler
{
    [HandlerContract("registernick")]
    public sealed class RegisterNickHandler : Abstraction.BaseClass.CmdHandlerBase
    {
        private new RegisterNickRequest _request => (RegisterNickRequest)base._request;
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
            catch (Exception e)
            {
                throw new GPDatabaseException(e.Message);
            }
        }
    }
}
