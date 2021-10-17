﻿using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Contract;
using PresenceConnectionManager.Entity.Structure.Request.Profile;
using PresenceSearchPlayer.Entity.Exception.General;
using System;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    [HandlerContract("registernick")]
    internal sealed class RegisterNickHandler : Abstraction.BaseClass.CmdHandlerBase
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