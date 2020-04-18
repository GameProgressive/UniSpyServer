using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Database.DatabaseModel.MySql;
using PresenceConnectionManager.Enumerator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Handler.Profile.RegisterNick
{
    public class RegisterNickHandler : PCMCommandHandlerBase
    {
        public RegisterNickHandler(IClient client, Dictionary<string, string> recv) : base(client, recv)
        {
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();

            if (!_recv.ContainsKey("sesskey"))
            {
                _errorCode = GPErrorCode.Parse;
            }

            if (!_recv.ContainsKey("uniquenick"))
            {
                _errorCode = GPErrorCode.Parse;
            }
        }

        protected override void DataOperation()
        {
            try
            {
                using (var db = new retrospyContext())
                {
                    db.Subprofiles.Where(s => s.Profileid == _session.UserInfo.Profileid && s.Namespaceid == _session.UserInfo.NamespaceID)
                        .First().Uniquenick = _recv["uniquenick"];
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                _errorCode = GPErrorCode.DatabaseError;
            }
        }

        protected override void ConstructResponse()
        {
            _sendingBuffer = @"\rn\final\";
        }
    }
}
