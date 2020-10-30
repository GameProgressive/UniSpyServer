using GameSpyLib.Abstraction.Interface;
using GameSpyLib.Database.DatabaseModel.MySql;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using PresenceSearchPlayer.Entity.Structure.Request;
using System.Collections.Generic;
using System.Linq;

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
                    _errorCode = GPError.CheckBadMail;
                    return;
                }

                if (db.Users.Where(u => u.Email == _request.Email && u.Password == _request.Password).Count() < 1)
                {
                    _errorCode = GPError.CheckBadPassword;
                    return;
                }

                var result = from p in db.Profiles
                             join u in db.Users on p.Userid equals u.Userid
                             where u.Email.Equals(_request.Email)
                             && u.Password.Equals(_request.Password)
                             && p.Nick.Equals(_request.Nick)
                             select p.Profileid;

                if (result.Count() == 1)
                {
                    _profileid = result.First();
                }
                else
                {
                    _errorCode = GPError.CheckBadNick;
                }
            }
        }

        protected override void BuildNormalResponse()
        {
            _sendingBuffer = @$"\cur\0\pid\{_profileid}\final\";
        }

        protected override void BuildErrorResponse()
        {
            if (_errorCode < GPError.Check || _errorCode > GPError.CheckBadPassword)
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
