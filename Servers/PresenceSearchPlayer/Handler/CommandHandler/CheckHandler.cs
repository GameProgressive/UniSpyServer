using System.Collections.Generic;
using System.Linq;
using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Database.DatabaseModel.MySql;
using PresenceSearchPlayer.Entity.Enumerator;
using PresenceSearchPlayer.Entity.Structure.Request;

namespace PresenceSearchPlayer.Handler.CommandHandler.Check
{
    public class CheckHandler : PSPCommandHandlerBase
    {
        // \check\\nick\<nick>\email\<email>\partnerid\0\passenc\<passenc>\gamename\gmtest\final\
        //\cur\pid\<pid>\final
        //check is request recieved correct and convert password into our MD5 type
        protected CheckRequest _request;
        uint _profileid;
        public CheckHandler(ISession client, Dictionary<string, string> recv) : base(client, recv)
        {
            _request = new CheckRequest(recv);
        }

        protected override void RequestCheck()
        {
            _errorCode = _request.Parse();
        }

        protected override void DataOperation()
        {
            using (var db = new retrospyContext())
            {
                if (db.Users.Where(e => e.Email == _request.Email).Count() < 1)
                {
                    _errorCode = GPErrorCode.CheckBadMail;
                    return;
                }

                if (db.Users.Where(u => u.Email == _request.Email && u.Password == _request.PassEnc).Count() < 1)
                {
                    _errorCode = GPErrorCode.CheckBadPassword;
                    return;
                }

                var result = from p in db.Profiles
                             join u in db.Users on p.Userid equals u.Userid
                             where u.Email.Equals(_request.Email)
                             && u.Password.Equals(_request.PassEnc)
                             && p.Nick.Equals(_request.Nick)
                             select p.Profileid;

                if (result.Count() == 1)
                {
                    _profileid = result.First();
                }
                else
                {
                    _errorCode = GPErrorCode.CheckBadNick;
                }
            }
        }

        protected override void BuildNormalResponse()
        {
            _sendingBuffer = @$"\cur\0\pid\{_profileid}\final\";
        }

        protected override void BuildErrorResponse()
        {
            if (_errorCode < GPErrorCode.Check || _errorCode > GPErrorCode.CheckBadPassword)
            {
                base.BuildErrorResponse();
            }
            else
            {
                _sendingBuffer = @$"\cur\{ _errorCode}\final\";
            }
        }
    }
}
