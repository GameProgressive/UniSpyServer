using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using PresenceSearchPlayer.Entity.Structure.Request;
using System.Collections.Generic;
using System.Linq;

namespace PresenceSearchPlayer.Handler.CmdHandler
{
    public class CheckHandler : PSPCommandHandlerBase
    {
        // \check\\nick\<nick>\email\<email>\partnerid\0\passenc\<passenc>\gamename\gmtest\final\
        //\cur\pid\<pid>\final
        //check is request recieved correct and convert password into our MD5 type
        protected new CheckRequest _request
        {
            get { return (CheckRequest)base._request; }
        }
        uint _profileid;
        public CheckHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
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

                if (db.Users.Where(u => u.Email == _request.Email && u.Password == _request.Password).Count() < 1)
                {
                    _errorCode = GPErrorCode.CheckBadPassword;
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
