using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Database.DatabaseModel.MySql;
using GameSpyLib.MiscMethod;
using PresenceSearchPlayer.Enumerator;
using System.Collections.Generic;
using System.Linq;

namespace PresenceSearchPlayer.Handler.CommandHandler.Check
{
    public class CheckHandler : PSPCommandHandlerBase
    {
        // \check\\nick\<nick>\email\<email>\partnerid\0\passenc\<passenc>\gamename\gmtest\final\
        //\cur\pid\<pid>\final
        //check is request recieved correct and convert password into our MD5 type

        public CheckHandler(ISession client, Dictionary<string, string> recv) : base(client, recv)
        {
        }

        protected override void CheckRequest()
        {
            if (!_recv.ContainsKey("nick") || !_recv.ContainsKey("email") || !_recv.ContainsKey("passenc"))
            {
                _errorCode = GPErrorCode.Parse;
            }

            if (!GameSpyUtils.IsEmailFormatCorrect(_recv["email"]))
            {
                _errorCode = GPErrorCode.CheckBadMail;
            }
        }

        protected override void DataOperation()
        {
            using (var db = new retrospyContext())
            {
                if (db.Users.Where(e => e.Email == _recv["email"]).Count() < 1)
                {
                    _errorCode = GPErrorCode.CheckBadMail;
                    return;
                }

                if (db.Users.Where(u => u.Email == _recv["email"] && u.Password == _recv["passenc"]).Count() < 1)
                {
                    _errorCode = GPErrorCode.CheckBadPassword;
                    return;
                }

                var result = from p in db.Profiles
                             join u in db.Users on p.Userid equals u.Userid
                             where u.Email.Equals(_recv["email"])
                             && u.Password.Equals(_recv["passenc"])
                             && p.Nick.Equals(_recv["nick"])
                             select p.Profileid;

                if (result.Count() == 1)
                {
                    _sendingBuffer = @$"\cur\0\pid\{result.FirstOrDefault()}\final\";
                }
                else
                {
                    _errorCode = GPErrorCode.CheckBadNick;
                }
            }
        }

        protected override void ConstructResponse()
        {
            if (_errorCode != GPErrorCode.NoError)
            {
                _sendingBuffer = @$"\cur\{ _errorCode}\final\";
            }
        }
    }
}
