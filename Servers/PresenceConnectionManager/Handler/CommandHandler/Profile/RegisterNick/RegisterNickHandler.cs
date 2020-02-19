using GameSpyLib.Database.DatabaseModel.MySql;
using LinqToDB;
using PresenceConnectionManager.Enumerator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Handler.Profile.RegisterNick
{
    public class RegisterNickHandler : GPCMHandlerBase
    {
        public RegisterNickHandler(Dictionary<string, string> recv) : base(recv)
        {
        }

        protected override void CheckRequest(GPCMSession session)
        {
            base.CheckRequest(session);
            if (!_recv.ContainsKey("sesskey"))
            {
                _errorCode = GPErrorCode.Parse;
            }
            if (!_recv.ContainsKey("uniquenick"))
            {
                _errorCode = GPErrorCode.Parse;
            }

        }

        protected override void DataBaseOperation(GPCMSession session)
        {
            try
            {
                using (var db = new RetrospyDB())
                {
                    db.Subprofiles.Where(s => s.Profileid == session.UserInfo.Profileid && s.Namespaceid == session.UserInfo.NamespaceID)
                        .Set(s => s.Uniquenick, _recv["uniquenick"])
                        .Update();
                }
            }
            catch (Exception e)
            {
                _errorCode = GPErrorCode.DatabaseError;
            }

        }

        protected override void ConstructResponse(GPCMSession session)
        {
            _sendingBuffer = @"\rn\final\";
        }
    }
}
