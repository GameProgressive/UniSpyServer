using GameSpyLib.Database.DatabaseModel.MySql;
using PresenceConnectionManager.Enumerator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Handler.Profile.RegisterNick
{
    public class RegisterNickHandler : CommandHandlerBase
    {
        public RegisterNickHandler() : base()
        {
        }

        protected override void CheckRequest(GPCMSession session, Dictionary<string, string> recv)
        {
            base.CheckRequest(session, recv);

            if (!recv.ContainsKey("sesskey"))
            {
                _errorCode = GPErrorCode.Parse;
            }

            if (!recv.ContainsKey("uniquenick"))
            {
                _errorCode = GPErrorCode.Parse;
            }
        }

        protected override void DataOperation(GPCMSession session, Dictionary<string, string> recv)
        {
            try
            {
                using (var db = new retrospyContext())
                {
                    db.Subprofiles.Where(s => s.Profileid == session.UserInfo.Profileid && s.Namespaceid == session.UserInfo.NamespaceID)
                        .First().Uniquenick = recv["uniquenick"];
                    db.SaveChanges();
                }
            }
            catch (Exception)
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
