using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;
using Chat.Entity.Exception;
using Chat.Entity.Structure.Request.General;
using Chat.Entity.Structure.Response.General;
using Chat.Entity.Structure.Result.General;
using System.Linq;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;

namespace Chat.Handler.CmdHandler.General
{
    [HandlerContract("LOGIN")]
    internal sealed class LoginHandler : CmdHandlerBase
    {

        private new LoginRequest _request => (LoginRequest)base._request;
        private new LoginResult _result
        {
            get => (LoginResult)base._result;
            set => base._result = value;
        }
        public LoginHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new LoginResult();
        }

        protected override void RequestCheck()
        {
            /// TODO: Verify which games does send a GS encoded password and not md5
            //we decoded gamespy encoded password then get md5 of it 
            //_password = GameSpyUtils.DecodePassword(_request.PasswordHash);
            //_password = StringExtensions.GetMD5Hash(_password);
        }

        protected override void DataOperation()
        {
            switch (_request.RequestType)
            {
                case LoginType.NickAndEmailLogin:
                    NickAndEmailLogin();
                    break;
                case LoginType.UniqueNickLogin:
                    UniqueNickLogin();
                    break;
            }
        }
        public void NickAndEmailLogin()
        {
            using (var db = new unispyContext())
            {
                var result = from u in db.Users
                             join p in db.Profiles on u.Userid equals p.Userid
                             where u.Email == _request.Email
                             && p.Nick == _request.NickName
                             && u.Password == _request.PasswordHash
                             select new
                             {
                                 userid = u.Userid,
                                 profileid = p.Profileid,
                                 emailVerified = u.Emailverified,
                                 banned = u.Banned
                             };

                if (result.Count() != 1)
                {
                    throw new Exception($"Can not find user with nickname:{_request.NickName} in database.");
                }
                _result.ProfileID = result.First().profileid;
                _result.UserID = result.First().userid;
            }
        }
        public void UniqueNickLogin()
        {
            using (var db = new unispyContext())
            {
                var result = from n in db.Subprofiles
                             join p in db.Profiles on n.Profileid equals p.Profileid
                             join u in db.Users on p.Userid equals u.Userid
                             where n.Uniquenick == _request.UniqueNick
                             && n.Namespaceid == _request.NameSpaceID
                             select new
                             {
                                 userid = u.Userid,
                                 profileid = p.Profileid,
                                 uniquenick = n.Uniquenick,
                                 emailVerified = u.Emailverified,
                                 banned = u.Banned
                             };
                if (result.Count() != 1)
                {
                    throw new Exception($"Can not find user with uniquenick:{_request.UniqueNick} in database.");
                }
                _result.ProfileID = result.First().profileid;
                _result.UserID = result.First().userid;
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new LoginResponse(_request, _result);
        }
    }
}
