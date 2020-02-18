using GameSpyLib.Database.DatabaseModel.MySql;
using PresenceSearchPlayer.Enumerator;
using System.Collections.Generic;
using System.Linq;

/////////////////////////Finished/////////////////////////////////
namespace PresenceSearchPlayer.Handler.CommandHandler.Nick
{

    /// <summary>
    /// Uses a email and namespaceid to find all nick in this account
    /// </summary>
    public class NickHandler : GPSPHandlerBase
    {
        public NickHandler(Dictionary<string, string> recv) : base(recv)
        {
        }

        protected override void CheckRequest(GPSPSession session)
        {
            base.CheckRequest(session);
            if (!_recv.ContainsKey("email"))
            {
                _errorCode = GPErrorCode.Parse;

            }
            // First, we try to receive an encoded password
            if (!_recv.ContainsKey("passenc"))
            {
                // If the encoded password is not sended, we try receiving the password in plain text
                if (!_recv.ContainsKey("pass"))
                {
                    // No password is specified, we cannot continue                   
                    _errorCode = GPErrorCode.Parse;
                }
            }
        }

        protected override void DataBaseOperation(GPSPSession session)
        {
            try
            {
                using (var db = new RetrospyDB())
                {
                    var players = from u in db.Users
                                  join p in db.Profiles on u.Userid equals p.Userid
                                  join n in db.Subprofiles on p.Profileid equals n.Profileid
                                  where u.Email == _recv["email"] && u.Password == _recv["passenc"] && n.Namespaceid == _namespaceid
                                  select new { nick = p.Nick, uniquenick = n.Uniquenick };
                    if (players.Count() == 0)
                    {
                        _errorCode = GPErrorCode.CheckBadPassword;
                    }
                    _sendingBuffer = @"\nr\";
                    foreach (var info in players)
                    {
                        _sendingBuffer += @"\nick\";
                        _sendingBuffer += info.nick;
                        _sendingBuffer += @"\uniquenick\";
                        _sendingBuffer += info.uniquenick;
                    }
                    _sendingBuffer += @"\ndone\final\";
                }
            }
            catch
            {
                _errorCode = GPErrorCode.DatabaseError;
            }


        }

        protected override void ConstructResponse(GPSPSession session)
        {
            if (_errorCode != GPErrorCode.NoError)
            {
                _sendingBuffer = @"\nr\\ndone\final\";
            }
        }
    }
}
