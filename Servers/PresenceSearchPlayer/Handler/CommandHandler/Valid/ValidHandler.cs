using GameSpyLib.Common;
using GameSpyLib.Database.DatabaseModel.MySql;
using PresenceSearchPlayer.Enumerator;
using System.Collections.Generic;
using System.Linq;

namespace PresenceSearchPlayer.Handler.CommandHandler.Valid
{
    public class ValidHandler : CommandHandlerBase
    {
        public ValidHandler(GPSPSession session, Dictionary<string, string> recv) : base(session, recv)
        {
        }

        protected override void CheckRequest(GPSPSession session, Dictionary<string, string> recv)
        {
            if (!recv.ContainsKey("email"))
            {
                _errorCode = GPErrorCode.Parse;
                return;
            }

            if (!GameSpyUtils.IsEmailFormatCorrect(recv["email"]))
            {
                _errorCode = GPErrorCode.Parse;
                return;
            }
        }

        protected override void DataOperation(GPSPSession session, Dictionary<string, string> recv)
        {
            using (var db = new retrospyContext())
            {
                var result = from u in db.Users
                             join p in db.Profiles on u.Userid equals p.Userid
                             join n in db.Subprofiles on p.Profileid equals n.Profileid
                             //According to FSW partnerid is not nessesary
                             where u.Email == recv["email"] && n.Gamename == recv["gamename"] && n.Namespaceid == _namespaceid
                             select p.Profileid;

                if (result.Count() == 0)
                {
                    _sendingBuffer = @"\vr\0\final\";

                }
                else if (result.Count() == 1)
                {
                    _sendingBuffer = @"\vr\1\final\";
                }
                else
                {
                    _errorCode = GPErrorCode.DatabaseError;
                }
            }
        }
    }
}
