using GameSpyLib.Common;
using GameSpyLib.Logging;
using PresenceSearchPlayer.Enumerator;
using GameSpyLib.Database.DatabaseModel.MySql;
using System.Collections.Generic;
using System.Linq;

namespace PresenceSearchPlayer.Handler.CommandHandler.Valid
{
    public class ValidHandler : GPSPHandlerBase
    {
        public ValidHandler(Dictionary<string, string> recv) : base(recv)
        {
        }
        uint _partnerid;
        protected override void CheckRequest(GPSPSession session)
        {
            if (!_recv.ContainsKey("email"))
            {
                _errorCode = GPErrorCode.Parse;
                return;
            }
            if (!GameSpyUtils.IsEmailFormatCorrect(_recv["email"]))
            {
                _errorCode = GPErrorCode.Parse;
                return;
            }
            if (!_recv.ContainsKey("partnerid") && !uint.TryParse(_recv["partnerid"], out _partnerid))
            {
                _errorCode = GPErrorCode.Parse;
                return;
            }
        }
        protected override void DataBaseOperation(GPSPSession session)
        {
            using (var db = new RetrospyDB())
            {
                var result = from u in db.Users
                             join p in db.Profiles on u.Userid equals p.Userid
                             join n in db.Namespaces on p.Profileid equals n.Profileid
                             where u.Email == _recv["email"] && p.Nick == _recv["nick"] && n.Partnerid == _partnerid && n.Gamename == _recv["gamename"]
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
