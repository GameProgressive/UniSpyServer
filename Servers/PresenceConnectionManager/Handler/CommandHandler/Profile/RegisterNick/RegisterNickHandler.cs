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
        public RegisterNickHandler(GPCMSession session,Dictionary<string, string> recv) : base(session,recv)
        {
        }

        protected override void CheckRequest(GPCMSession session, Dictionary<string, string> recv)
        {
            base.CheckRequest(session,recv);
            if (!recv.ContainsKey("sesskey"))
            {
                _errorCode = GPErrorCode.Parse;
            }
            if (!recv.ContainsKey("uniquenick"))
            {
                _errorCode = GPErrorCode.Parse;
            }

        }

        protected override void DataBaseOperation(GPCMSession session, Dictionary<string, string> recv)
        {
            try
            {
                using (var db = new RetrospyDB())
                {
                    db.Subprofiles.Where(s => s.Profileid == session.UserInfo.Profileid && s.Namespaceid == session.UserInfo.NamespaceID)
                        .Set(s => s.Uniquenick, recv["uniquenick"])
                        .Update();
                }
            }
            catch (Exception e)
            {
                _errorCode = GPErrorCode.DatabaseError;
            }

        }

        protected override void ConstructResponse(GPCMSession session, Dictionary<string, string> recv)
        {
            _sendingBuffer = @"\rn\final\";
        }
    }
}
