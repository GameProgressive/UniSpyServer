using GameSpyLib.Common;
using GameSpyLib.Database.DatabaseModel.MySql;
using PresenceSearchPlayer.Enumerator;
using System.Collections.Generic;
using System.Linq;

namespace PresenceSearchPlayer.Handler.CommandHandler.Check
{
    public class CheckHandler : GPSPHandlerBase
    {
        // \check\\nick\<nick>\email\<email>\partnerid\0\passenc\<passenc>\gamename\gmtest\final\
        //\cur\pid\<pid>\final
        //check is request recieved correct and convert password into our MD5 type
        public CheckHandler(GPSPSession session, Dictionary<string, string> recv) : base(session, recv)
        {
        }
        protected override void CheckRequest(GPSPSession session, Dictionary<string, string> recv)
        {
            if (!recv.ContainsKey("nick") || !recv.ContainsKey("email") || !recv.ContainsKey("passenc"))
            {
                _errorCode = GPErrorCode.Parse;
            }
            if (!GameSpyUtils.IsEmailFormatCorrect(recv["email"]))
            {
                _errorCode = GPErrorCode.CheckBadMail;
            }
        }



        protected override void DataBaseOperation(GPSPSession session, Dictionary<string, string> recv)
        {
            using (var db = new RetrospyDB())
            {
                if (db.Users.Where(e => e.Email == recv["email"]).Count() < 1)
                {
                    _errorCode = GPErrorCode.CheckBadMail;
                    return;
                }

                if (db.Users.Where(u => u.Email == recv["email"] && u.Password == recv["passenc"]).Count() < 1)
                {
                    _errorCode = GPErrorCode.CheckBadPassword;
                    return;
                }

                var result = from p in db.Profiles
                             join u in db.Users on p.Userid equals u.Userid
                             where u.Email.Equals(recv["email"])
                             && u.Password.Equals(recv["passenc"])
                             && p.Nick.Equals(recv["nick"])
                             select p.Profileid;

                if (result.Count() == 1)
                {
                    _sendingBuffer = @"\cur\0\pid\" + result.First() + @"\final\";
                }
                else
                {
                    _errorCode = GPErrorCode.CheckBadNick;
                }
            }
        }
        protected override void ConstructResponse(GPSPSession session, Dictionary<string, string> recv)
        {
            if (_errorCode != GPErrorCode.NoError)
            {
                _sendingBuffer = @"\cur\" + (uint)_errorCode + @"\final\";
            }
        }
    }
}
