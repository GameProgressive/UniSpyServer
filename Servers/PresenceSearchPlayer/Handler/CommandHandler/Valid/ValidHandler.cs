using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Database.DatabaseModel.MySql;
using GameSpyLib.MiscMethod;
using PresenceSearchPlayer.Enumerator;
using System.Collections.Generic;
using System.Linq;

namespace PresenceSearchPlayer.Handler.CommandHandler.Valid
{
    public class ValidHandler : PSPCommandHandlerBase
    {
        public ValidHandler(ISession client, Dictionary<string, string> recv) : base(client, recv)
        {
        }

        protected override void CheckRequest()
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
        }

        protected override void DataOperation()
        {
            try
            {
                using (var db = new retrospyContext())
                {
                    var result = from u in db.Users
                                 join p in db.Profiles on u.Id equals p.Userid
                                 join n in db.Subprofiles on p.Id equals n.Profileid
                                 //According to FSW partnerid is not nessesary
                                 where u.Email == _recv["email"]
                                 && n.Gamename == _recv["gamename"]
                                 && n.Namespaceid == _namespaceid
                                 select p.Id;

                    if (result.Count() == 0)
                    {
                        _sendingBuffer = @"\vr\0\final\";

                    }
                    else if (result.Count() == 1)
                    {
                        _sendingBuffer = @"\vr\1\final\";
                    }

                }
            }
            catch
            {
                _errorCode = GPErrorCode.DatabaseError;
            }

        }
    }
}
